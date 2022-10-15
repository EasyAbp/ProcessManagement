using Volo.Abp.Reflection;

namespace EasyAbp.ProcessManagement.Permissions;

public class ProcessManagementPermissions
{
    public const string GroupName = "EasyAbp.ProcessManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ProcessManagementPermissions));
    }
}
