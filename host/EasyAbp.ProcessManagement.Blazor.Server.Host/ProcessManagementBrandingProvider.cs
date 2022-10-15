using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace EasyAbp.ProcessManagement.Blazor.Server.Host;

[Dependency(ReplaceServices = true)]
public class ProcessManagementBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "ProcessManagement";
}
