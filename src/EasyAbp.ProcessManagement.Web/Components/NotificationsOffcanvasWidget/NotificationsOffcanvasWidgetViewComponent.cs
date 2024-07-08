using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Web.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace EasyAbp.ProcessManagement.Web.Components.NotificationsOffcanvasWidget;

[Widget(
    AutoInitialize = true,
    RefreshUrl = "/Widgets/ProcessManagement/NotificationsOffcanvas",
    ScriptFiles = ["/Components/NotificationsOffcanvasWidget/Default.js"]
)]
public class NotificationsOffcanvasWidgetViewComponent : AbpViewComponent
{
    protected ProcessManagementWebOptions Options =>
        LazyServiceProvider.LazyGetRequiredService<IOptions<ProcessManagementWebOptions>>().Value;

    public NotificationsOffcanvasWidgetViewComponent()
    {
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        return View("~/Components/NotificationsOffcanvasWidget/Default.cshtml",
            new NotificationsOffcanvasWidgetViewModel(Options));
    }
}