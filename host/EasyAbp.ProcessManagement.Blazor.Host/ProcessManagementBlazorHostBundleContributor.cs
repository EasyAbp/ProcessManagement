using Volo.Abp.Bundling;

namespace EasyAbp.ProcessManagement.Blazor.Host;

public class ProcessManagementBlazorHostBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {

    }

    public void AddStyles(BundleContext context)
    {
        context.Add("main.css", true);
    }
}
