using System.Threading.Tasks;
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

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.
        context.Menu.AddItem(new ApplicationMenuItem(ProcessManagementMenus.Prefix, displayName: "ProcessManagement", icon: "fa fa-list", url: "~/ProcessManagement"));

        return Task.CompletedTask;
    }
}
