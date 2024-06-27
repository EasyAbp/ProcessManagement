using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.ProcessManagement.Web.Components.Toolbar.Notifications;

public class NotificationsViewComponent : AbpViewComponent
{
    public virtual Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(View("~/Components/Toolbar/Notifications/Default.cshtml"));
    }
}