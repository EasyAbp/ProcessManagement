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
        //Add main menu items.
        context.Menu.GetAdministration().AddItem(new ApplicationMenuItem(ProcessManagementMenus.Prefix,
            displayName: "ProcessManagement", icon: "fa fa-list", url: "~/ProcessManagement"));

        if (await context.IsGrantedAsync(ProcessManagementPermissions.Process.Manage))
        {
            context.Menu.GetAdministration().AddItem(
                new ApplicationMenuItem(ProcessManagementMenus.Process, l["Menu:Process"],
                    "/ProcessManagement/Processes/Process")
            );
        }
    }
}