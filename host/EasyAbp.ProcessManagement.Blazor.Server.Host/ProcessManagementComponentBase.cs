using EasyAbp.ProcessManagement.Localization;
using Volo.Abp.AspNetCore.Components;

namespace EasyAbp.ProcessManagement.Blazor.Server.Host;

public abstract class ProcessManagementComponentBase : AbpComponentBase
{
    protected ProcessManagementComponentBase()
    {
        LocalizationResource = typeof(ProcessManagementResource);
    }
}
