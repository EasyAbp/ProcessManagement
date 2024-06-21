using System;
using System.Threading.Tasks;

namespace EasyAbp.ProcessManagement.UserGroups;

public interface IUserGroupContributor
{
    /// <summary>
    /// Get and update the specific user's GroupKeys. 
    /// </summary>
    Task UpdateAsync(Guid userId);
}