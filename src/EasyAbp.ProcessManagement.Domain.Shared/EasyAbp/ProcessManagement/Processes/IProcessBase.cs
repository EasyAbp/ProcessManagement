using System;

namespace EasyAbp.ProcessManagement.Processes;

public interface IProcessBase
{
    /// <summary>
    /// The hardcoded Name value from ProcessDefinition.
    /// </summary>
    string ProcessName { get; }

    /// <summary>
    /// A unique correlation ID. If not set, this value will be auto-set to the value of the Id property.
    /// </summary>
    string CorrelationId { get; }

    /// <summary>
    /// A custom user group key. It can be used for auth and filter.
    /// </summary>
    /// <example>
    /// OU:{OrganizationUnitId}
    /// </example>
    string GroupKey { get; }

    /// <summary>
    /// Time of this process completed.
    /// </summary>
    DateTime? CompletionTime { get; }
}