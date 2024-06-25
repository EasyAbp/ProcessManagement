using System;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public class ProcessStateHistory : AggregateRoot<Guid>, IProcessState, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid ProcessId { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string? SubStateName { get; protected set; }

    /// <inheritdoc/>
    public virtual ProcessStateFlag StateFlag { get; protected set; }

    /// <inheritdoc/>
    public string? StateSummaryText { get; protected set; }

    /// <inheritdoc/>
    public virtual string? StateDetailsText { get; protected set; }

    /// <inheritdoc/>
    public virtual DateTime StateUpdateTime { get; protected set; }

    protected ProcessStateHistory()
    {
    }

    public ProcessStateHistory(Guid id, Process process) : base(id)
    {
        TenantId = process.TenantId;

        ProcessId = process.Id;

        StateUpdateTime = process.StateUpdateTime;
        StateName = process.StateName;
        SubStateName = process.SubStateName;
        StateFlag = process.StateFlag;
        StateSummaryText = process.StateSummaryText;
        StateDetailsText = process.StateDetailsText;
    }
}