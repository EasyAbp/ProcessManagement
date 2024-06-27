using System;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.UserGroups;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.UserGroups;

public class UserGroupRepositoryTests : ProcessManagementEntityFrameworkCoreTestBase
{
    private readonly IUserGroupRepository _userGroupRepository;

    public UserGroupRepositoryTests()
    {
        _userGroupRepository = GetRequiredService<IUserGroupRepository>();
    }

    /*
    [Fact]
    public async Task Test1()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            // Arrange

            // Act

            //Assert
        });
    }
    */
}
