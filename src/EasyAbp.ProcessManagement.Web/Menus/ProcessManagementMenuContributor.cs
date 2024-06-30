using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Localization;
using EasyAbp.ProcessManagement.Permissions;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.ProcessManagement.Web.Menus;

public class ProcessManagementMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<ProcessManagementResource>();

        var processManagementMenuItem = new ApplicationMenuItem(ProcessManagementMenus.Prefix,
            displayName: l["Menu:ProcessManagement"], icon: "fa fa-tasks", url: "~/ProcessManagement");

        if (await context.IsGrantedAsync(ProcessManagementPermissions.Process.Manage))
        {
            processManagementMenuItem.AddItem(
                new ApplicationMenuItem(ProcessManagementMenus.Process, l["Menu:Process"],
                    "/ProcessManagement/Processes/Process")
            );
        }

        if (processManagementMenuItem.Items.Any())
        {
            context.Menu.GetAdministration().AddItem(processManagementMenuItem);
        }
    }
}