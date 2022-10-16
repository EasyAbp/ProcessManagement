using System;
using System.Collections.Generic;
using EasyAbp.ProcessManagement.Options;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.Processes;

public class Process : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Record the Name value of <see cref="ProcessDefinition"/>.
    /// </summary>
    [NotNull]
    public virtual string ProcessName { get; protected set; }

    /// <summary>
    /// A custom tag. It can be used for auth.
    /// </summary>
    /// <example>
    /// {OrganizationUnitId}+{UserId}
    /// </example>
    [CanBeNull]
    public virtual string CustomTag { get; protected set; }

    /// <summary>
    /// Name of the current state. It can be used for search.
    /// </summary>
    [NotNull]
    public virtual string CurrentStateName { get; protected set; }

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
        [CanBeNull] string customTag = null) : base(id)
    {
        TenantId = tenantId;
        CustomTag = customTag;
        ProcessName = processDefinition.Name;

        StateHistories = new List<ProcessStateHistory>();

        SetState(processDefinition, processDefinition.InitialStateName, now);
    }

    internal void SetState(ProcessDefinition processDefinition, string stateName, DateTime now)
    {
        CheckProcessDefinition(processDefinition);
        CheckIsNotCompleted();

        CurrentStateName = stateName;
        StateHistories.Add(new ProcessStateHistory(Id, StateHistories.Count, stateName, now));
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