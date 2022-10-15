using EasyAbp.ProcessManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace EasyAbp.ProcessManagement.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class ProcessManagementPageModel : AbpPageModel
{
    protected ProcessManagementPageModel()
    {
        LocalizationResourceType = typeof(ProcessManagementResource);
        ObjectMapperContext = typeof(ProcessManagementWebModule);
    }
}
