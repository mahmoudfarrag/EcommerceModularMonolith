using ECommerce.Shared.Abstractions;
using System.Reflection;

namespace Ecommerce.Host;

public static class HostModulesApiRegistration
{
    public static IMvcBuilder AddModuleControllers(this IMvcBuilder builder, ConfigurationManager configuration)
    {
        /*
                 * .AddApplicationPart(typeof(ProductController).Assembly)
            .AddApplicationPart(typeof(OrderController).Assembly);
         */
        //var assemblies = Directory.GetFiles(AppContext.BaseDirectory, "*.Api.dll")
        //    .Select(Assembly.LoadFrom);

        //foreach (var assembly in assemblies)
        //{
        //    builder.AddApplicationPart(assembly);
        //}

        //return builder;


    
 

        var modulesPath = configuration["ModulesPath"];


        if (!Directory.Exists(modulesPath))
        {
            throw new DirectoryNotFoundException(
                $"The 'Modules' folder was not found at path: {modulesPath}. " 
            );
        }

        //Load all module API assemblies from folder
        var dllFiles = Directory.GetFiles(modulesPath, "*.Api.dll", SearchOption.TopDirectoryOnly);

        if (dllFiles.Length == 0)
        {
            throw new FileNotFoundException(
                $"No module API assemblies (*.Api.dll) found in folder: {modulesPath}. " +
                "Make sure your module projects copy their API DLLs to this folder."
            );
        }

        foreach (var file in dllFiles)
        {
            var assembly = Assembly.LoadFrom(file);
            builder.AddApplicationPart(assembly);
            Console.WriteLine($"Loaded controllers from module: {Path.GetFileName(file)}");
        }

        return builder;
    }

  
}
