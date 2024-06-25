using Volo.Abp.Reflection;

namespace EasyAbp.ProcessManagement.Permissions;

public class ProcessManagementPermissions
{
    public const string GroupName = "EasyAbp.ProcessManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ProcessManagementPermissions));
    }
    public class Process
    {
        public const string Default = GroupName + ".Process";
        public const string Manage = Default + ".Manage";
    }
}
