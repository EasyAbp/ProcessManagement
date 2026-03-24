using System;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications.Dtos;

namespace EasyAbp.ProcessManagement.Notifications;

/// <summary>
/// Pushes real-time notification events to connected clients.
/// Implement this interface in the presentation layer (e.g., via SignalR).
/// </summary>
public interface INotificationPushService
{
    Task PushNewNotificationAsync(Guid userId, NotificationDto notification);
}
