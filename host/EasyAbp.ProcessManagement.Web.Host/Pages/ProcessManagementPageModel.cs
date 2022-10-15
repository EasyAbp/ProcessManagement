using EasyAbp.ProcessManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EasyAbp.ProcessManagement.Pages;

public abstract class ProcessManagementPageModel : AbpPageModel
{
    protected ProcessManagementPageModel()
    {
        LocalizationResourceType = typeof(ProcessManagementResource);
    }
}
