using Volo.Abp.Localization;

namespace EasyAbp.ProcessManagement.Web.Options;

public class ProcessStateActionDefinition
{
    /// <summary>
    /// The hardcoded Name value from ProcessDefinition.
    /// </summary>
    public string ProcessName { get; set; } = null!;

    /// <summary>
    /// The hardcoded state name defined by the backend.
    /// </summary>
    public string StateName { get; set; } = null!;

    /// <summary>
    /// Display name.
    /// </summary>
    public ILocalizableString DisplayName { get; set; } = null!;

    /// <summary>
    /// JS code that is executed when the table action button is clicked.
    /// </summary>
    /// <example>detailsModal.open({id: data.record.id});</example>
    public string TableOnClickCallbackCode { get; set; } = null!;

    /// <summary>
    /// JS code that is executed when the offcanvas action button is clicked.
    /// </summary>
    /// <example>detailsModal.open({id: data.id});</example>
    public string OffcanvasOnClickCallbackCode { get; set; } = null!;

    /// <summary>
    /// JS code for the action visible check. Skip checking if null.
    /// </summary>
    /// <example>abp.auth.isGranted('MyPermissionName') && data.available</example>
    public string? VisibleCheckCode { get; set; }

    public ProcessStateActionDefinition(string processName, string stateName, ILocalizableString displayName,
        string tableOnClickCallbackCode, string offcanvasOnClickCallbackCode, string? visibleCheckCode)
    {
        ProcessName = processName;
        StateName = stateName;
        DisplayName = displayName;
        TableOnClickCallbackCode = tableOnClickCallbackCode;
        OffcanvasOnClickCallbackCode = offcanvasOnClickCallbackCode;
        VisibleCheckCode = visibleCheckCode;
    }
}