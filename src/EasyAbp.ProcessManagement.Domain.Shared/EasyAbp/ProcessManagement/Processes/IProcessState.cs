using System;
using JetBrains.Annotations;

namespace EasyAbp.ProcessManagement.Processes;

public interface IProcessState
{
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

    /// <summary>
    /// Description for the state, optional. It supports both plain text and HTML.
    /// </summary>
    /// <example><![CDATA[<p>Data is loading...</p>]]></example>
    [CanBeNull]
    string DetailsText { get; }

    /// <summary>
    /// Time of the process has been updated to this state.
    /// </summary>
    DateTime StateUpdateTime { get; }
}