using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace EasyAbp.ProcessManagement.Blazor.Menus;

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
        context.Menu.GetAdministration().AddItem(new ApplicationMenuItem(ProcessManagementMenus.Prefix,
            displayName: "ProcessManagement", icon: "fa fa-list"));

        return Task.CompletedTask;
    }
}
