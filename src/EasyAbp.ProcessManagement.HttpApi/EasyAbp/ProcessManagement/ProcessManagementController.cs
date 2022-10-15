using EasyAbp.ProcessManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.ProcessManagement;

public abstract class ProcessManagementController : AbpControllerBase
{
    protected ProcessManagementController()
    {
        LocalizationResource = typeof(ProcessManagementResource);
    }
}
