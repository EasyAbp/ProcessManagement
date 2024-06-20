using System;
using JetBrains.Annotations;

namespace EasyAbp.ProcessManagement.Processes;

public interface IProcess
{
    /// <summary>
    /// The hardcoded Name value from ProcessDefinition.
    /// </summary>
    [NotNull]
    string ProcessName { get; }

    /// <summary>
    /// An unique correlation ID. If not set, this value will be auto-set to the value of the Id property.
    /// </summary>
    [NotNull]
    string CorrelationId { get; }

    /// <summary>
    /// A custom tag. It can be used for auth and filter.
    /// </summary>
    /// <example>
    /// {OrganizationUnitId}
    /// </example>
    [CanBeNull]
    string CustomTag { get; }

    /// <summary>
    /// Record whether this process changed into a final state.
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// Time of this process completed.
    /// </summary>
    DateTime? CompletionTime { get; }
}