using EasyAbp.ProcessManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EasyAbp.ProcessManagement.Permissions;

public class ProcessManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ProcessManagementPermissions.GroupName, L("Permission:ProcessManagement"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ProcessManagementResource>(name);
    }
}
