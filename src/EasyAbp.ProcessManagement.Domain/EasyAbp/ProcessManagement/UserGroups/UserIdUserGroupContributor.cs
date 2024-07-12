using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace EasyAbp.ProcessManagement.UserGroups;

public class UserIdUserGroupContributor : UserGroupContributorBase, IDistributedEventHandler<EntityCreatedEto<UserEto>>
{
    public override string GroupKeyPrefix => "U:";

    protected override Task<List<Guid>> InternalGetUserIdsAsync(string originalKey)
    {
        return Task.FromResult<List<Guid>>([Guid.Parse(originalKey)]);
    }

    public override Task<List<string>> GetUserGroupKeysAsync(Guid userId)
    {
        return Task.FromResult<List<string>>([$"{GroupKeyPrefix}{userId}"]);
    }

    public virtual async Task HandleEventAsync(EntityCreatedEto<UserEto> eventData)
    {
        await UpdateAsync(eventData.Entity.Id);
    }

    public virtual Task<string> CreateGroupKeyAsync(Guid userId) => CreateGroupKeyAsync(userId.ToString());
}