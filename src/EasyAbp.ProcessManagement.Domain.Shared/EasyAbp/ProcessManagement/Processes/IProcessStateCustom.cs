namespace EasyAbp.ProcessManagement.Processes;

public interface IProcessStateCustom : IProcessStateCustomBase
{
    /// <summary>
    /// Description for the state, optional. This value will NOT be shown in the notification list,
    /// users can only view it in the detail page/modal. Both plain text and HTML are supported.
    /// </summary>
    /// <example><![CDATA[<p>Data is loading, the uploader ID is...</p>]]></example>
    string? StateDetailsText { get; }
}