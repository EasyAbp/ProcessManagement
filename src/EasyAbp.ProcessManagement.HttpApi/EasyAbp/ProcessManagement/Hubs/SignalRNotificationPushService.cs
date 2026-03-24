using System;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.ProcessManagement.Hubs;

/// <summary>
/// Pushes notification events to connected clients via SignalR.
/// Replaces <see cref="NullNotificationPushService"/> when the HttpApi module is loaded.
/// </summary>
public class SignalRNotificationPushService : INotificationPushService, ITransientDependency
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationPushService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task PushNewNotificationAsync(Guid userId, NotificationDto notification)
    {
        await _hubContext.Clients
            .User(userId.ToString())
            .SendAsync("ReceiveNotification", notification);
    }
}
