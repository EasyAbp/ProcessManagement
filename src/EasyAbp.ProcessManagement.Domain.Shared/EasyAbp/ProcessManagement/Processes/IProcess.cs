using System;
using JetBrains.Annotations;

namespace EasyAbp.ProcessManagement.Processes;

public interface IProcess : IProcessStateBase
{
    /// <summary>
    /// The hardcoded Name value from ProcessDefinition.
    /// </summary>
    [NotNull]
    string ProcessName { get; }

    /// <summary>
    /// A unique correlation ID. If not set, this value will be auto-set to the value of the Id property.
    /// </summary>
    [NotNull]
    string CorrelationId { get; }

    /// <summary>
    /// A custom user group key. It can be used for auth and filter.
    /// </summary>
    /// <example>
    /// OU:{OrganizationUnitId}
    /// </example>
    [CanBeNull]
    string GroupKey { get; }

    /// <summary>
    /// Record whether this process changed into a final state.
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// Time of this process completed.
    /// </summary>
    DateTime? CompletionTime { get; }
}