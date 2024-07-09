using EasyAbp.ProcessManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EasyAbp.ProcessManagement.Permissions;

public class DemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var demoGroup = context.AddGroup(DemoPermissions.GroupName, L("Permission:Demo"));

        var exportingPermission =
            demoGroup.AddPermission(DemoPermissions.Exporting.Default, L("Permission:Exporting"));

        exportingPermission.AddChild(DemoPermissions.Exporting.Retry, L("Permission:Retry"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DemoResource>(name);
    }
}