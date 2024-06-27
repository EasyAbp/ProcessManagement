using System;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.Notifications;

public class Notification : CreationAuditedAggregateRoot<Guid>, IProcess, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid UserId { get; protected set; }

    public virtual Guid ProcessId { get; protected set; }

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
    public virtual DateTime? CompletionTime { get; protected set; }

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

    #endregion

    protected Notification()
    {
    }

    public Notification(Guid id, ProcessEto process, Guid userId) : base(id)
    {
        TenantId = process.TenantId;
        UserId = userId;
        ProcessId = process.Id;

        ProcessName = process.ProcessName;

        CorrelationId = process.CorrelationId;
        GroupKey = process.GroupKey;
        CompletionTime = process.CompletionTime;

        StateUpdateTime = process.StateUpdateTime;
        StateName = process.StateName;
        ActionName = process.ActionName;
        StateFlag = process.StateFlag;
        StateSummaryText = process.StateSummaryText;
    }

    public void SetRead(bool read)
    {
        Read = read;
    }

    public void SetDismissed(bool dismissed)
    {
        Dismissed = dismissed;
    }
}