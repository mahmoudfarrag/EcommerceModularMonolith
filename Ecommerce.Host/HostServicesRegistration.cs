using Ecommerce.Host.Models;
using ECommerce.Shared.Abstractions;
using ECommerce.Shared.Messaging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Reflection;

namespace Ecommerce.Host;

public static class HostServicesRegistration
{
    private static readonly List<ModuleInfo> _loadedModules = new();
    public static IReadOnlyList<ModuleInfo> LoadedModules => _loadedModules;
    public static IServiceCollection AddHostServices_way1_not_used(this IServiceCollection services, string connectionString)
    {
        
        ConfigureRabitMq(services);

        ConfigureSwagger(services);


        // Register each module (DbContexts + services)
        //services.AddCatalogModule(connectionString);
        //services.AddOrdersModule(connectionString);
        //services.AddPaymentsModule(connectionString);

        services.AddSingleton<IMessageBus, RabbitMqMessageBus>();

        return services;
    }
    public static IServiceCollection AddHostServices(this IServiceCollection services, string connectionString, ConfigurationManager configuration)
    {

        ConfigureRabitMq(services);

        ConfigureSwagger(services);


        // Register each module (DbContexts + services)
        RegisterModulesServices(services, connectionString,configuration);

        services.AddSingleton<IMessageBus, RabbitMqMessageBus>();

        return services;
    }

    private static void ConfigureRabitMq(IServiceCollection services)
    {
        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                DispatchConsumersAsync = true // for async consumers
            };

            return factory.CreateConnection();
        });
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        // Register Swagger
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce API", Version = "v1" });
        });
    }

    private static void RegisterModulesServices(IServiceCollection services, string connectionString, ConfigurationManager configuration)
    {
        //var modulesPath = Path.Combine(AppContext.BaseDirectory, "Modules");
      var modulesPath =  configuration["ModulesPath"];
        


        if (!Directory.Exists(modulesPath))
        {
            throw new DirectoryNotFoundException($"The 'Modules' folder was not found at path: {modulesPath}.");
        }

        Console.WriteLine("Start Loading all module assemblies from: " + modulesPath);

        List<Assembly> loadedAssemblies = new List<Assembly>();

        //  Load *all* DLLs — including Api, Infrastructure, ModuleDefinition, etc.
        foreach (var dllPath in Directory.GetFiles(modulesPath, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                loadedAssemblies.Add(assembly);
                Console.WriteLine($"Loaded assembly: {Path.GetFileName(dllPath)}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not load {Path.GetFileName(dllPath)}: {ex.Message}");
            }
        }

        #region TODO performloading for api assemblies here instead of read dlls twice
        //var apiAssemblies = loadedAssemblies
        //      .Where(a => a.FullName != null && a.FullName.Contains("Api"))
        //      .ToList();

        //  if (apiAssemblies.Count == 0)
        //  {
        //      throw new FileNotFoundException(
        //          $"No module API assemblies (*.Api.dll) found in folder: {modulesPath}" 
        //      );
        //  }

        //  foreach (var assembly in apiAssemblies)
        //  {
        //      builder.AddApplicationPart(assembly);
        //      Console.WriteLine($"Loaded controllers from module: {Path.GetFileName(assembly.FullName)}");
        //  }

        #endregion

        // ✅ Now get all assemblies that contain modules
        var moduleAssemblies = loadedAssemblies
            .Where(a => a.FullName != null && a.FullName.Contains("ModuleDefinition"))
            .ToList();

        // ✅ Find all types implementing IModule
        var moduleTypes = moduleAssemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();

        if (!moduleTypes.Any())
        {
            throw new FileNotFoundException("No modules implementing IModule were found in loaded assemblies.");
           
        }

        Console.WriteLine($"Found {moduleTypes.Count} module(s): " +
                          string.Join(", ", moduleTypes.Select(t => t.Name)));

        // ✅ Sort them by dependencies
        var sortedModules = ModuleLoader.SortModulesByDependencies(moduleTypes);

        Console.WriteLine("Module load order: " + string.Join(" -> ", sortedModules.Select(t => t.Name)));

        // ✅ Register modules one by one in dependency order
        foreach (var moduleType in sortedModules)
        {
            try
            {
                var module = (IModule)Activator.CreateInstance(moduleType)!;
                module.Register(services, connectionString);
                Console.WriteLine($"Registered module: {moduleType.Name}");

                var depends = moduleType
                            .GetCustomAttributes(typeof(DependsOnAttribute), true)
                            .Cast<DependsOnAttribute>()
                            .SelectMany(a => a.Dependencies)
                            .Select(t => t.Name)
                            .ToList();

                _loadedModules.Add(new ModuleInfo
                {
                    Name = moduleType.Name,
                    AssemblyName = moduleType.Assembly.GetName().Name ?? "Unknown",
                    Dependencies = depends
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to register {moduleType.Name}: {ex.Message}");
                throw;
            }
        }
    }


}
