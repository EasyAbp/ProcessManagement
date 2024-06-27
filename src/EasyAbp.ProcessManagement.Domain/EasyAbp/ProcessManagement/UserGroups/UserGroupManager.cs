using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace EasyAbp.ProcessManagement.UserGroups;

public class UserGroupManager : IUserGroupManager, ITransientDependency
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;

    [UnitOfWork(true)]
    public virtual async Task UpdateAsync(Guid userId)
    {
        var contributors = LazyServiceProvider.LazyGetRequiredService<IEnumerable<IUserGroupContributor>>();

        foreach (var contributor in contributors)
        {
            await contributor.UpdateAsync(userId);
        }
    }

    public virtual async Task<List<Guid>> GetUserIdsAsync(string groupKey)
    {
        var contributors = LazyServiceProvider.LazyGetRequiredService<IEnumerable<IUserGroupContributor>>();

        List<Guid> userIds = [];

        foreach (var contributor in contributors)
        {
            userIds.AddRange(await contributor.GetUserIdsAsync(groupKey));
        }

        return userIds.Distinct().ToList();
    }
}