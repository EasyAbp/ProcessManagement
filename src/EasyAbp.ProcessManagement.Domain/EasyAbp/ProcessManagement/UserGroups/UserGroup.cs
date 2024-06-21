using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.UserGroups;

public class UserGroup : CreationAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid UserId { get; protected set; }

    [NotNull]
    public virtual string GroupKey { get; protected set; }

    protected UserGroup()
    {
    }

    public UserGroup(Guid id, Guid? tenantId, Guid userId, [NotNull] string groupKey) : base(id)
    {
        TenantId = tenantId;
        UserId = userId;
        GroupKey = Check.NotNullOrWhiteSpace(groupKey, nameof(groupKey));
    }
}