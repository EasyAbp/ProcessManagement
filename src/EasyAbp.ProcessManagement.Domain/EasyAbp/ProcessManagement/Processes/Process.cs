using System;
using System.Collections.Generic;
using EasyAbp.ProcessManagement.Options;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.Processes;

public class Process : FullAuditedAggregateRoot<Guid>, IProcessState, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// The hardcoded Name value from <see cref="ProcessDefinition"/>.
    /// </summary>
    [NotNull]
    public virtual string ProcessName { get; protected set; }

    /// <summary>
    /// An unique correlation ID. If not set, this value will be auto-set to the value of the Id property.
    /// </summary>
    [NotNull]
    public virtual string CorrelationId { get; protected set; }

    /// <summary>
    /// A custom tag. It can be used for auth and filter.
    /// </summary>
    /// <example>
    /// {OrganizationUnitId}
    /// </example>
    [CanBeNull]
    public virtual string CustomTag { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string SubStateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string DetailsText { get; protected set; }

    /// <inheritdoc/>
    public virtual DateTime StateUpdateTime { get; protected set; }

    /// <summary>
    /// History collection of state changes.
    /// </summary>
    public virtual List<ProcessStateHistory> StateHistories { get; protected set; }

    /// <summary>
    /// Record whether this process changed into a final state.
    /// </summary>
    public virtual bool IsCompleted { get; protected set; }

    /// <summary>
    /// Time of this process completed.
    /// </summary>
    public virtual DateTime CompletionTime { get; protected set; }

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

        SetState(processDefinition, new ProcessStateInfoModel(processDefinition.InitialStateName, null, null, now));
    }

    public Process(Guid id, Guid? tenantId, ProcessDefinition processDefinition, IProcessState processState,
        [CanBeNull] string correlationId = null, [CanBeNull] string customTag = null) : base(id)
    {
        TenantId = tenantId;
        CorrelationId = correlationId ?? id.ToString();
        CustomTag = customTag;
        ProcessName = processDefinition.Name;

        StateHistories = new List<ProcessStateHistory>();

        SetState(processDefinition, processState);
    }

    internal void SetState(ProcessDefinition processDefinition, IProcessState processState)
    {
        CheckProcessDefinition(processDefinition);
        CheckIsNotCompleted();

        StateHistories.Add(new ProcessStateHistory(Id, this));

        StateName = processState.StateName;
        SubStateName = processState.SubStateName;
        DetailsText = processState.DetailsText;
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