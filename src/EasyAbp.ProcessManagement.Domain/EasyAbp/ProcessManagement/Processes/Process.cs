using System;
using System.Collections.Generic;
using EasyAbp.ProcessManagement.Options;
using JetBrains.Annotations;
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
    public virtual string CustomTag { get; protected set; }

    /// <inheritdoc/>
    public virtual bool IsCompleted { get; protected set; }

    /// <inheritdoc/>
    public virtual DateTime? CompletionTime { get; protected set; }

    /// <inheritdoc/>
    public virtual DateTime StateUpdateTime { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string SubStateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateDetailsText { get; protected set; }

    /// <summary>
    /// History collection of state changes.
    /// </summary>
    public virtual List<ProcessStateHistory> StateHistories { get; protected set; }

    protected Process()
    {
    }

    public Process(Guid id, Guid? tenantId, ProcessDefinition processDefinition, DateTime now,
        [CanBeNull] string correlationId = null, [CanBeNull] string customTag = null) : base(id)
    {
        TenantId = tenantId;
        CorrelationId = correlationId ?? id.ToString();
        CustomTag = customTag;
        ProcessName = processDefinition.Name;

        StateHistories = new List<ProcessStateHistory>();

        SetState(processDefinition, new ProcessStateInfoModel(now, processDefinition.InitialStateName, null, null));
    }

    public Process(Guid id, Guid? tenantId, ProcessDefinition processDefinition, IProcessState processState,
        [CanBeNull] string correlationId = null, [CanBeNull] string customTag = null) : base(id)
    {
        TenantId = tenantId;
        CorrelationId = correlationId ?? id.ToString();
        CustomTag = customTag;
        ProcessName = processDefinition.Name;

        StateHistories = [];

        SetState(processDefinition, processState);
    }

    internal void SetState(ProcessDefinition processDefinition, IProcessState processState)
    {
        CheckProcessDefinition(processDefinition);
        CheckIsNotCompleted();

        StateHistories.Add(new ProcessStateHistory(Id, this));

        StateName = processState.StateName;
        SubStateName = processState.SubStateName;
        StateDetailsText = processState.StateDetailsText;
        StateUpdateTime = processState.StateUpdateTime;
    }

    internal void CompleteProcess(DateTime now)
    {
        CheckIsNotCompleted();

        IsCompleted = true;
        CompletionTime = now;
    }

    private void CheckProcessDefinition(ProcessDefinition processDefinition)
    {
        if (processDefinition.Name != ProcessName)
        {
            throw new AbpException($"Invalid process definition. The expected process's name is `{ProcessName}`");
        }
    }

    private void CheckIsNotCompleted()
    {
        if (IsCompleted)
        {
            throw new AbpException(
                $"The operation failed since process `{ProcessName}` (id: {Id}) had already been completed.");
        }
    }
}