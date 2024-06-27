using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.ProcessManagement.UserGroups;

public interface IUserGroupContributor
{
    /// <summary>
    /// e.g. "U:" is the group key prefix for the <see cref="UserIdUserGroupContributor"/>.
    /// </summary>
    string GroupKeyPrefix { get; }

    /// <summary>
    /// Get and update the specific user's GroupKeys. 
    /// </summary>
    Task UpdateAsync(Guid userId);

    /// <summary>
    /// Create a GroupKey for the Process entity.
    /// </summary>
    /// <param name="originalKey">e.g. the userId value (054BD4FC-835B-48EC-8472-087FFD6D6C95) is an originalKey for the <see cref="UserIdUserGroupContributor"/>.</param>
    /// <returns>e.g. <see cref="UserIdUserGroupContributor"/> creates a key: U:054BD4FC-835B-48EC-8472-087FFD6D6C95</returns>
    Task<string> CreateGroupKeyAsync(string originalKey);

    /// <summary>
    /// Get user IDs by groupKey.
    /// </summary>
    Task<List<Guid>> GetUserIdsAsync(string groupKey);
}