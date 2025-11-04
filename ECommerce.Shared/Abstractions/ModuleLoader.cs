using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.Shared.Abstractions;

public static class ModuleLoader
{
    public static List<Type> SortModulesByDependencies(List<Type> moduleTypes)
    {
        var sorted = new List<Type>();
        var visited = new HashSet<Type>();

        foreach (var moduleType in moduleTypes)
        {
            Visit(moduleType, moduleTypes, sorted, visited);
        }
        Console.WriteLine($"📦 Module load order: {string.Join(" -> ", sorted.Select(x => x.Name))}");

        return sorted;
    }

    private static void Visit(Type moduleType, List<Type> allModules, List<Type> sorted, HashSet<Type> visited)
    {
        if (visited.Contains(moduleType))
            return;

        visited.Add(moduleType);

        // Dynamically read the [DependsOn(...)] attribute
        var dependsOnAttr = moduleType.GetCustomAttribute<DependsOnAttribute>();
        if (dependsOnAttr != null)
        {
            foreach (var dependency in dependsOnAttr.Dependencies)
            {
                // find the type of that dependency in the loaded module types
                var depType = allModules.FirstOrDefault(m => m.Name == dependency.Name);
                if (depType != null)
                {
                    Visit(depType, allModules, sorted, visited);
                }
            }
        }

        sorted.Add(moduleType);
    }
}
