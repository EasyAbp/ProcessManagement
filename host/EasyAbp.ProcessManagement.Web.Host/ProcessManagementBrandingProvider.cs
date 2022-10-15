using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.ProcessManagement;

[Dependency(ReplaceServices = true)]
public class ProcessManagementBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "ProcessManagement";
}
