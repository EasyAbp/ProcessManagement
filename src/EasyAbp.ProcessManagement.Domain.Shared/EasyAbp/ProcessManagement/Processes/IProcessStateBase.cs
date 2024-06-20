using System;
using JetBrains.Annotations;

namespace EasyAbp.ProcessManagement.Processes;

public interface IProcessStateBase
{
    /// <summary>
    /// Time of the process has been updated to this state.
    /// </summary>
    DateTime StateUpdateTime { get; }

    /// <summary>
    /// The hardcoded state name defined by the backend.
    /// </summary>
    /// <example>Processing</example>
    [NotNull]
    string StateName { get; }

    /// <summary>
    /// A dynamic custom sub state name, optional. Users can see both the StateName and SubStateName on the UI.
    /// </summary>
    /// <example>Copying the data</example>
    [CanBeNull]
    string SubStateName { get; }
}