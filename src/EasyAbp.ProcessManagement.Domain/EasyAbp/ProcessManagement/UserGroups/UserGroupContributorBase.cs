using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace EasyAbp.ProcessManagement.UserGroups;

public abstract class UserGroupContributorBase : IUserGroupContributor, ITransientDependency
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;

    protected abstract Task<List<string>> GetUserGroupKeysAsync(Guid userId);

    [UnitOfWork(true)]
    public virtual async Task UpdateAsync(Guid userId)
    {
        await InternalUpdateAsync(userId, await GetUserGroupKeysAsync(userId));
    }

    protected virtual async Task InternalUpdateAsync(Guid userId, List<string> userGroupKeys)
    {
        // todo: try to find existing entities and update.
        // todo: insert new entities.
        // todo: remove unused entities.
    }
}