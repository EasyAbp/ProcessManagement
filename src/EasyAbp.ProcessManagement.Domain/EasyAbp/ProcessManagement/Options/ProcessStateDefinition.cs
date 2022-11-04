using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Options;

public class ProcessStateDefinition
{
    [NotNull]
    public string Name { get; }

    [CanBeNull]
    public string DisplayName { get; }

    internal HashSet<string> NextStateNames { get; } = new();

    public ProcessStateDefinition([NotNull] string name, [CanBeNull] string displayName)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        DisplayName = displayName;
    }
}