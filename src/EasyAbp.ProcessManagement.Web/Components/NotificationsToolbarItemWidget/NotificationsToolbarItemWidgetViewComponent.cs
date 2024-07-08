using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Web.Caches;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace EasyAbp.ProcessManagement.Web.Components.NotificationsToolbarItemWidget;

[Widget(
    AutoInitialize = true,
    RefreshUrl = "/Widgets/ProcessManagement/NotificationsToolbarItem"
)]
public class NotificationsToolbarItemWidgetViewComponent : AbpViewComponent
{
    private readonly NotificationCountCache _notificationCountCache;

    public NotificationsToolbarItemWidgetViewComponent(NotificationCountCache notificationCountCache)
    {
        _notificationCountCache = notificationCountCache;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var notificationCount = await _notificationCountCache.GetOrAddAsync();

        return View("~/Components/NotificationsToolbarItemWidget/Default.cshtml",
            new NotificationsToolbarItemWidgetViewModel(notificationCount));
    }
}