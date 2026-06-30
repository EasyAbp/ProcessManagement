using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyAbp.ProcessManagement.UserGroups;

/// <summary>
/// A test-only contributor that resolves a single group key into several fixed users, so tests can
/// exercise the multi-user notification fan-out (the default <see cref="UserIdUserGroupContributor"/>
/// only ever maps a key to one user). Inert for any key that does not use its prefix.
/// </summary>
public class TestGroupUserGroupContributor : UserGroupContributorBase
{
    public const string GroupKey = "T:multi";

    public static readonly IReadOnlyList<Guid> UserIds =
    [
        Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Guid.Parse("33333333-3333-3333-3333-333333333333")
    ];

    public override string GroupKeyPrefix => "T:";

    protected override Task<List<Guid>> InternalGetUserIdsAsync(string originalKey)
    {
        return Task.FromResult(originalKey == "multi" ? UserIds.ToList() : []);
    }

    public override Task<List<string>> GetUserGroupKeysAsync(Guid userId)
    {
        return Task.FromResult<List<string>>(UserIds.Contains(userId) ? [GroupKey] : []);
    }
}
