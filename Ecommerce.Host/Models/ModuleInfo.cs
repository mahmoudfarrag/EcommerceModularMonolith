namespace Ecommerce.Host.Models;

public class ModuleInfo
{
    public string Name { get; set; } = string.Empty;
    public List<string> Dependencies { get; set; } = new();
    public string AssemblyName { get; set; } = string.Empty;
    public DateTime LoadedAt { get; set; } = DateTime.UtcNow;
}
