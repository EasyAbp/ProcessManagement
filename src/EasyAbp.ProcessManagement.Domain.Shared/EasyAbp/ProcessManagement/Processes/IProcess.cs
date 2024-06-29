namespace EasyAbp.ProcessManagement.Processes;

public interface IProcess : IProcessBase, IProcessStateBase
{
    /// <summary>
    /// A unique correlation ID. If not set, this value will be auto-set to the value of the Id property.
    /// </summary>
    string CorrelationId { get; }
}