using System;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public class ProcessStateHistory : AggregateRoot<Guid>, IProcessState, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid ProcessId { get; protected set; }

    public virtual string ProcessName { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string? ActionName { get; protected set; }

    /// <inheritdoc/>
    public virtual ProcessStateFlag StateFlag { get; protected set; }

    /// <inheritdoc/>
    public virtual string? StateSummaryText { get; protected set; }

    /// <inheritdoc/>
    public virtual DateTime StateUpdateTime { get; protected set; }

    protected ProcessStateHistory()
    {
    }

    public ProcessStateHistory(Guid id, Guid? tenantId, Guid processId, string processName, IProcessState state) :
        base(id)
    {
        TenantId = tenantId;

        ProcessId = processId;
        ProcessName = processName;

        StateUpdateTime = state.StateUpdateTime;
        StateName = state.StateName;
        ActionName = state.ActionName;
        StateFlag = state.StateFlag;
        StateSummaryText = state.StateSummaryText;
    }
}