using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.ProcessManagement.Web.Controllers;

[Route("Widgets/ProcessManagement")]
public class ProcessManagementWidgetsController : AbpController
{
    [HttpGet]
    [Route("Notifications")]
    public IActionResult Notifications()
    {
        return ViewComponent("NotificationsWidget");
    }
}