using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.UserGroups;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.UserGroups;

public class UserGroupRepository : EfCoreRepository<IProcessManagementDbContext, UserGroup, Guid>, IUserGroupRepository
{
    public UserGroupRepository(IDbContextProvider<IProcessManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<UserGroup>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}