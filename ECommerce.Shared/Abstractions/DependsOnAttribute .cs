using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Abstractions;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DependsOnAttribute : Attribute
{
    public Type[] Dependencies { get; }

    public DependsOnAttribute(params Type[] dependencies)
    {
        Dependencies = dependencies;
    }
}
