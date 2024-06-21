using System;
using System.Collections.Generic;
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
}