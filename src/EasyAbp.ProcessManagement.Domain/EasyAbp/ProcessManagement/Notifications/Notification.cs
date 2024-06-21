using System;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.Notifications;

public class Notification : CreationAuditedAggregateRoot<Guid>, IProcess, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid UserId { get; protected set; }

    public virtual bool Read { get; protected set; }

    public virtual bool Dismissed { get; protected set; }

    #region Process information

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

    #endregion

    public Notification(Guid id, Process process, Guid userId) : base(id)
    {
        TenantId = process.TenantId;
        UserId = userId;

        ProcessName = process.ProcessName;

        CorrelationId = process.CorrelationId;
        GroupKey = process.GroupKey;
        IsCompleted = process.IsCompleted;
        CompletionTime = process.CompletionTime;

        StateUpdateTime = process.StateUpdateTime;
        StateName = process.StateName;
        SubStateName = process.SubStateName;
        StateFlag = process.StateFlag;
        StateSummaryText = process.StateSummaryText;
    }

    public void SetAsRead(bool dismissed)
    {
        Read = true;
        Dismissed = dismissed;
    }
}