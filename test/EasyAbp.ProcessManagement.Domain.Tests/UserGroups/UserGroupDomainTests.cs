using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace EasyAbp.ProcessManagement.UserGroups;

public class UserGroupDomainTests : ProcessManagementDomainTestBase
{
    private readonly IUserGroupManager _userGroupManager;

    public UserGroupDomainTests()
    {
        _userGroupManager = GetRequiredService<IUserGroupManager>();
    }

    [Fact]
    public async Task Default_Contributor_Should_Resolve_The_User_Id_From_Its_Group_Key()
    {
        var userId = Guid.NewGuid();

        var userIds = await _userGroupManager.GetUserIdsAsync($"U:{userId}");

        userIds.ShouldContain(userId);
    }

    [Fact]
    public async Task Default_Contributor_Should_Resolve_The_Group_Key_For_A_User()
    {
        var userId = Guid.NewGuid();

        var groupKeys = await _userGroupManager.GetUserGroupKeysAsync(userId);

        groupKeys.ShouldContain($"U:{userId}");
    }

    [Fact]
    public async Task Should_Aggregate_User_Ids_From_All_Contributors()
    {
        // The custom test contributor maps a single key to several users.
        var userIds = await _userGroupManager.GetUserIdsAsync(TestGroupUserGroupContributor.GroupKey);

        userIds.ShouldBe(TestGroupUserGroupContributor.UserIds, ignoreOrder: true);
    }

    [Fact]
    public async Task Should_Return_Empty_For_An_Unknown_Group_Key_Prefix()
    {
        var userIds = await _userGroupManager.GetUserIdsAsync("unknown-prefix-key");

        userIds.ShouldBeEmpty();
    }
}
