using System.Collections.Generic;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Options;

public class ProcessStateDefinition
{
    public string Name { get; }

    public string? DisplayName { get; }

    internal HashSet<string> NextStateNames { get; } = new();

    public ProcessStateDefinition(string name, string? displayName)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        DisplayName = displayName;
    }
}