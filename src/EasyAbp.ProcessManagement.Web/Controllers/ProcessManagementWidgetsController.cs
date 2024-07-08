using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.ProcessManagement.Web.Controllers;

[Route("Widgets/ProcessManagement")]
public class ProcessManagementWidgetsController : AbpController
{
    [HttpGet]
    [Route("NotificationsToolbarItem")]
    public IActionResult NotificationsToolbarItem()
    {
        return ViewComponent("NotificationsToolbarItemWidget");
    }
}