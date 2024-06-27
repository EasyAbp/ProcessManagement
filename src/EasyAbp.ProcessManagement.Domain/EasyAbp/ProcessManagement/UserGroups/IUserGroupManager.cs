using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.ProcessManagement.UserGroups;

public interface IUserGroupManager
{
    /// <summary>
    /// Get and update the specific user's GroupKeys. 
    /// </summary>
    Task UpdateAsync(Guid userId);

    /// <summary>
    /// Get user IDs by groupKey.
    /// </summary>
    Task<List<Guid>> GetUserIdsAsync(string groupKey);
}