namespace EasyAbp.ProcessManagement.Processes;

public interface IProcessBase
{
    /// <summary>
    /// The hardcoded Name value from ProcessDefinition.
    /// </summary>
    string ProcessName { get; }

    /// <summary>
    /// A custom user group key. It can be used for auth and filter.
    /// </summary>
    /// <example>
    /// OU:{OrganizationUnitId}
    /// </example>
    string GroupKey { get; }
}