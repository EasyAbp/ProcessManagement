using System.Collections.Generic;
using System.Linq;

namespace EasyAbp.ProcessManagement.Options;

public class ProcessStateDefinition
{
    public string Name { get; }

    internal HashSet<string> NextStateNames { get; } = new();

    public ProcessStateDefinition(string name)
    {
        Name = name;
    }
}