using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Permissions;
using EasyAbp.ProcessManagement.Web.Caches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    protected IAuthorizationService AuthorizationService =>
        LazyServiceProvider.LazyGetRequiredService<IAuthorizationService>();

    private readonly NotificationCountCache _notificationCountCache;

    public NotificationsToolbarItemWidgetViewComponent(NotificationCountCache notificationCountCache)
    {
        _notificationCountCache = notificationCountCache;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        // The refresh endpoint can be called by anyone signed in; without the permission the notification
        // list query would throw, so render nothing instead.
        if (!await AuthorizationService.IsGrantedAsync(ProcessManagementPermissions.Process.Default))
        {
            return Content(string.Empty);
        }

        int notificationCount;

        if (HttpContext.Request.Query.TryGetValue("count", out var countValue) &&
            int.TryParse(countValue, out var parsedCount))
        {
            notificationCount = parsedCount;
        }
        else
        {
            notificationCount = await _notificationCountCache.GetOrAddAsync();
        }

        return View("~/Components/NotificationsToolbarItemWidget/Default.cshtml",
            new NotificationsToolbarItemWidgetViewModel(notificationCount));
    }
}