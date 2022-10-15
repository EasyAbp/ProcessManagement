using EasyAbp.ProcessManagement.Localization;
using Volo.Abp.Application.Services;

namespace EasyAbp.ProcessManagement;

public abstract class ProcessManagementAppService : ApplicationService
{
    protected ProcessManagementAppService()
    {
        LocalizationResource = typeof(ProcessManagementResource);
        ObjectMapperContext = typeof(ProcessManagementApplicationModule);
    }
}
