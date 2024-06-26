using System;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class ProcessStateChangedEto : IMultiTenant, IProcessBase
{
    public Guid? TenantId { get; set; }

    public Guid ProcessId { get; set; }

    public string ProcessName { get; set; }

    public string CorrelationId { get; set; }

    public string GroupKey { get; set; }

    public DateTime? CompletionTime { get; set; }

    public ProcessStateInfoModel? OldState { get; set; }

    public ProcessStateInfoModel NewState { get; set; }

    public ProcessStateChangedEto()
    {
    }

    public ProcessStateChangedEto(Guid? tenantId, Guid processId, string processName, string correlationId,
        string groupKey, DateTime? completionTime, ProcessStateInfoModel? oldState, ProcessStateInfoModel newState)
    {
        TenantId = tenantId;
        ProcessId = processId;
        ProcessName = processName;
        CorrelationId = correlationId;
        GroupKey = groupKey;
        CompletionTime = completionTime;
        OldState = oldState;
        NewState = newState;
    }
}