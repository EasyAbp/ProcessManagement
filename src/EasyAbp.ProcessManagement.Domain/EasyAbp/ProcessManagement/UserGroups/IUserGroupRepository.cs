using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.ProcessManagement.UserGroups;

public interface IUserGroupRepository : IRepository<UserGroup, Guid>
{
}
