using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace EasyAbp.ProcessManagement.UserGroups;

public abstract class UserGroupContributorBase : IUserGroupContributor, ITransientDependency
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;

    protected ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    protected IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetRequiredService<IGuidGenerator>();

    protected IUserGroupRepository UserGroupRepository =>
        LazyServiceProvider.LazyGetRequiredService<IUserGroupRepository>();

    public abstract Task<List<string>> GetUserGroupKeysAsync(Guid userId);

    public abstract string GroupKeyPrefix { get; }

    [UnitOfWork(true)]
    public virtual async Task UpdateAsync(Guid userId)
    {
        await InternalUpdateAsync(userId, await GetUserGroupKeysAsync(userId));
    }

    public virtual Task<string> CreateGroupKeyAsync(string originalKey) =>
        Task.FromResult<string>($"{GroupKeyPrefix}{originalKey}");

    public virtual async Task<List<Guid>> GetUserIdsAsync(string groupKey)
    {
        if (!groupKey.StartsWith(GroupKeyPrefix))
        {
            return [];
        }

        return await InternalGetUserIdsAsync(groupKey.RemovePreFix(GroupKeyPrefix));
    }

    protected abstract Task<List<Guid>> InternalGetUserIdsAsync(string originalKey);

    [UnitOfWork]
    protected virtual async Task InternalUpdateAsync(Guid userId, List<string> userGroupKeys)
    {
        var existingUserGroups = await UserGroupRepository.GetListAsync(x => x.UserId == userId);

        foreach (var newUserGroup in userGroupKeys.Where(x => !existingUserGroups.Select(y => y.GroupKey).Contains(x)))
        {
            await UserGroupRepository.InsertAsync(
                new UserGroup(GuidGenerator.Create(), CurrentTenant.Id, userId, newUserGroup), true);
        }

        foreach (var removingUserGroup in existingUserGroups.Where(x => !userGroupKeys.Contains(x.GroupKey)))
        {
            await UserGroupRepository.DeleteAsync(removingUserGroup, true);
        }
    }
}