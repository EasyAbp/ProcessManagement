using EasyAbp.ProcessManagement.Web.Options;

namespace EasyAbp.ProcessManagement.Web.Components.NotificationsOffcanvasWidget;

public class NotificationsOffcanvasWidgetViewModel
{
    public ProcessManagementWebOptions Options { get; }

    public NotificationsOffcanvasWidgetViewModel(ProcessManagementWebOptions options)
    {
        Options = options;
    }
}