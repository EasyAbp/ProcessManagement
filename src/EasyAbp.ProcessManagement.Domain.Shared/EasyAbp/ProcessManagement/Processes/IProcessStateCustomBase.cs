using System;

namespace EasyAbp.ProcessManagement.Processes;

public interface IProcessStateCustomBase
{
    /// <summary>
    /// A dynamic custom sub state name, optional. Users can see both the StateName and SubStateName on the UI.
    /// Only plain text is supported.
    /// </summary>
    /// <example>Copying the data</example>
    string? SubStateName { get; }

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
    string? StateSummaryText { get; }
}