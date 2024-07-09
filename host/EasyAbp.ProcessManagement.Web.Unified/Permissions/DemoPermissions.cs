using Volo.Abp.Reflection;

namespace EasyAbp.ProcessManagement.Permissions;

public class DemoPermissions
{
    public const string GroupName = "Demo";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(DemoPermissions));
    }

    public class Exporting
    {
        public const string Default = GroupName + ".Exporting";
        public const string Retry = Default + ".Retry";
    }
}