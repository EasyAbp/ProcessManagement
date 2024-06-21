using System;
using System.Threading.Tasks;

namespace EasyAbp.ProcessManagement.UserGroups;

public interface IUserGroupManager
{
    /// <summary>
    /// Get and update the specific user's GroupKeys. 
    /// </summary>
    Task UpdateAsync(Guid userId);
}