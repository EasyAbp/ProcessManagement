namespace EasyAbp.ProcessManagement;

public static class ProcessManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "EasyAbpProcessManagement";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "EasyAbpProcessManagement";
}
