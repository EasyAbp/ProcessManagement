using EasyAbp.ProcessManagement.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace EasyAbp.ProcessManagement;

[Area(ProcessManagementRemoteServiceConsts.ModuleName)]
public abstract class ProcessManagementController : AbpControllerBase
{
    protected ProcessManagementController()
    {
        LocalizationResource = typeof(ProcessManagementResource);
    }
}
