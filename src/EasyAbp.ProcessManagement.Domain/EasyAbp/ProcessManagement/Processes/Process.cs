using System;
using EasyAbp.ProcessManagement.Options;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.Processes;

public class Process : FullAuditedAggregateRoot<Guid>, IProcess, IProcessState, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <inheritdoc/>
    public virtual string ProcessName { get; protected set; }

    /// <inheritdoc/>
    public virtual string CorrelationId { get; protected set; }

    /// <inheritdoc/>
    public virtual string GroupKey { get; protected set; }

    /// <inheritdoc/>
    public virtual DateTime StateUpdateTime { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string? ActionName { get; protected set; }

    /// <inheritdoc/>
    public virtual ProcessStateFlag StateFlag { get; protected set; }

    /// <inheritdoc/>
    public virtual string? StateSummaryText { get; protected set; }

    /// <inheritdoc/>
    public virtual string? StateDetailsText { get; protected set; }

    protected Process()
    {
    }

    internal Process(Guid id, Guid? tenantId, ProcessDefinition processDefinition, DateTime now, string groupKey,
        string correlationId, IProcessStateCustom? stateCustom = null) : base(id)
    {
        TenantId = tenantId;
        CorrelationId = correlationId;
        GroupKey = groupKey;
        ProcessName = Check.NotNullOrWhiteSpace(processDefinition.Name, nameof(ProcessName));

        SetState(new ProcessStateInfoModel(now, processDefinition.InitialStateName, stateCustom));
    }

    internal void SetState(IProcessState processState)
    {
        var oldState = StateName.IsNullOrEmpty() ? null : ToStateInfoModel();

        StateUpdateTime = processState.StateUpdateTime;
        StateName = Check.NotNullOrWhiteSpace(processState.StateName, nameof(processState.StateName));
        ActionName = processState.ActionName;
        StateFlag = processState.StateFlag;
        StateSummaryText = processState.StateSummaryText;
        StateDetailsText = processState.StateDetailsText;

        AddLocalEvent(new ProcessStateChangedEto(
            TenantId, Id, ProcessName, CorrelationId, GroupKey, oldState, ToStateInfoModel()));
    }

    public ProcessStateInfoModel ToStateInfoModel()
    {
        return new ProcessStateInfoModel(StateUpdateTime, StateName, this);
    }
}