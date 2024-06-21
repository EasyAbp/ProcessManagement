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
    /// Only plain text is supported.
    /// </summary>
    /// <example>Copying the data</example>
    [CanBeNull]
    string SubStateName { get; }

    /// <summary>
    /// This flag is converted to a state icon and displayed on the UI.
    /// For example, when the flag is Warning, the UI shows ⚠️.
    /// </summary>
    ProcessStateFlag StateFlag { get; }

    /// <summary>
    /// Summary for the state, optional. This value will be shown in the notification list.
    /// Both plain text and HTML are supported.
    /// </summary>
    /// <example><![CDATA[<p>Data is loading...</p>]]></example>
    [CanBeNull]
    string StateSummaryText { get; }
}