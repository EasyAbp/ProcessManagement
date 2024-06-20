using System;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.UserProcesses;

public class UserProcess : CreationAuditedAggregateRoot<Guid>, IProcess, IProcessStateBase, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid UserId { get; protected set; }

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

    public UserProcess(Guid id, Process process, Guid userId) : base(id)
    {
        TenantId = process.TenantId;
        UserId = userId;

        Update(process);
    }

    public void Update(Process process)
    {
        ProcessName = process.ProcessName;
        CorrelationId = process.CorrelationId;
        CustomTag = process.CustomTag;
        IsCompleted = process.IsCompleted;
        CompletionTime = process.CompletionTime;

        StateUpdateTime = process.StateUpdateTime;
        StateName = process.StateName;
        SubStateName = process.SubStateName;
    }
}