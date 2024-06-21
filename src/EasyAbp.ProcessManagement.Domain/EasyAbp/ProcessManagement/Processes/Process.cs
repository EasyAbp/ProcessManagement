﻿using System;
using System.Collections.Generic;
using EasyAbp.ProcessManagement.Options;
using EasyAbp.ProcessManagement.ProcessStateHistories;
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
    public virtual string GroupKey { get; protected set; }

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
    public virtual ProcessStateFlag StateFlag { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateSummaryText { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateDetailsText { get; protected set; }

    protected Process()
    {
    }

    internal Process(Guid id, Guid? tenantId, ProcessDefinition processDefinition, IProcessState processState,
        [CanBeNull] string correlationId = null, [CanBeNull] string groupKey = null) : base(id)
    {
        TenantId = tenantId;
        CorrelationId = correlationId ?? id.ToString();
        GroupKey = groupKey;
        ProcessName = Check.NotNullOrWhiteSpace(processDefinition.Name, nameof(ProcessName));

        SetState(processDefinition, processState);
    }

    internal void SetState(ProcessDefinition processDefinition, IProcessState processState)
    {
        CheckProcessDefinition(processDefinition);
        CheckIsNotCompleted();

        var oldState = ToStateInfoModel();

        StateUpdateTime = processState.StateUpdateTime;
        StateName = Check.NotNullOrWhiteSpace(processState.StateName, nameof(processState.StateName));
        SubStateName = processState.SubStateName;
        StateFlag = processState.StateFlag;
        StateSummaryText = processState.StateSummaryText;
        StateDetailsText = processState.StateDetailsText;

        AddLocalEvent(new ProcessStateChangedEto(oldState, this));
    }

    public ProcessStateInfoModel ToStateInfoModel()
    {
        return new ProcessStateInfoModel(
            StateUpdateTime, StateName, SubStateName, StateFlag, StateSummaryText, StateDetailsText);
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