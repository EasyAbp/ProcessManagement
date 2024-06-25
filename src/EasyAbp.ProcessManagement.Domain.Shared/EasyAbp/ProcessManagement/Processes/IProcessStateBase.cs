using System;

namespace EasyAbp.ProcessManagement.Processes;

public interface IProcessStateBase : IProcessStateCustomBase
{
    /// <summary>
    /// Time of the process has been updated to this state.
    /// </summary>
    DateTime StateUpdateTime { get; }

    /// <summary>
    /// The hardcoded state name defined by the backend.
    /// </summary>
    /// <example>Processing</example>
    string StateName { get; }
}