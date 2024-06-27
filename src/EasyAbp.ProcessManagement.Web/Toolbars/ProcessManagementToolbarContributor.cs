using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Permissions;
using EasyAbp.ProcessManagement.Web.Components.Toolbar.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Users;

namespace EasyAbp.ProcessManagement.Web.Toolbars;

public class ProcessManagementToolbarContributor : IToolbarContributor
{
    public virtual Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name != StandardToolbars.Main)
        {
            return Task.CompletedTask;
        }

        var authenticated = context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated;

        if (authenticated)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(NotificationsViewComponent), 0,
                ProcessManagementPermissions.Process.Default));
        }

        return Task.CompletedTask;
    }
}