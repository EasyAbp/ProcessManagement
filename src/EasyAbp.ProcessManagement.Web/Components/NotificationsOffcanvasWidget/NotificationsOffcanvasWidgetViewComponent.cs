using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Permissions;
using EasyAbp.ProcessManagement.Web.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.SignalR;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace EasyAbp.ProcessManagement.Web.Components.NotificationsOffcanvasWidget;

[Widget(
    AutoInitialize = true,
    RefreshUrl = "/Widgets/ProcessManagement/NotificationsOffcanvas",
    ScriptTypes = [typeof(SignalRBrowserScriptContributor)],
    ScriptFiles = ["/Components/NotificationsOffcanvasWidget/Default.js"]
)]
public class NotificationsOffcanvasWidgetViewComponent : AbpViewComponent
{
    protected ProcessManagementWebOptions Options =>
        LazyServiceProvider.LazyGetRequiredService<IOptions<ProcessManagementWebOptions>>().Value;

    protected IAuthorizationService AuthorizationService =>
        LazyServiceProvider.LazyGetRequiredService<IAuthorizationService>();

    public NotificationsOffcanvasWidgetViewComponent()
    {
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        // The layout hook renders this widget on every page, so users without the permission get no markup at all
        // (and, with no #notificationsOffcanvas element, Default.js never opens the SignalR connection).
        if (!await AuthorizationService.IsGrantedAsync(ProcessManagementPermissions.Process.Default))
        {
            return Content(string.Empty);
        }

        return View("~/Components/NotificationsOffcanvasWidget/Default.cshtml",
            new NotificationsOffcanvasWidgetViewModel(Options));
    }
}