using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.ProcessManagement.Notifications;

public interface INotificationRepository : IRepository<Notification, Guid>
{
    /// <summary>
    /// Dismisses, in a single set-based database operation, all of the given user's
    /// not-yet-dismissed notifications created at or before <paramref name="maxCreationTime"/>.
    /// Entities are not loaded into memory, so it stays fast even with a large number of notifications.
    /// </summary>
    Task DismissAllAsync(
        Guid userId,
        DateTime maxCreationTime,
        DateTime dismissedTime,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Dismisses, in a single set-based database operation, all not-yet-dismissed notifications
    /// belonging to the given process. Entities are not loaded into memory, so it stays fast even
    /// when the process has notifications for a large number of users.
    /// </summary>
    Task DismissByProcessIdAsync(
        Guid processId,
        DateTime dismissedTime,
        CancellationToken cancellationToken = default);
}
