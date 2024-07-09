namespace EasyAbp.ProcessManagement.Processes;

public interface IProcessStateCustom
{
    /// <summary>
    /// A custom action name as a complement to the StateName, optional.
    /// Users can see both the StateName and ActionName on the UI.
    /// Only plain text is supported.
    /// </summary>
    /// <example>Copying the data</example>
    string? ActionName { get; }

    /// <summary>
    /// This flag is converted to a state icon and displayed on the UI.
    /// For example, when the flag is Warning, the UI shows ⚠️.
    /// </summary>
    ProcessStateFlag StateFlag { get; }

    /// <summary>
    /// Summary for the state, optional. This value will be shown in the notification list.
    /// Only plain text is supported.
    /// </summary>
    /// <example><![CDATA[<p>Data is loading...</p>]]></example>
    string? StateSummaryText { get; }
}