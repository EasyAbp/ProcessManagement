using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Web.Caches;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace EasyAbp.ProcessManagement.Web.Components.NotificationsWidget;

[Widget(
    AutoInitialize = true,
    RefreshUrl = "/Widgets/ProcessManagement/Notifications",
    ScriptFiles = ["/Components/NotificationsWidget/Default.js"]
)]
public class NotificationsWidgetViewComponent : AbpViewComponent
{
    private readonly NotificationCountCache _notificationCountCache;

    public NotificationsWidgetViewComponent(NotificationCountCache notificationCountCache)
    {
        _notificationCountCache = notificationCountCache;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var notificationCount = await _notificationCountCache.GetOrAddAsync();

        return View("~/Components/NotificationsWidget/Default.cshtml",
            new NotificationsWidgetViewModel(notificationCount));
    }
}