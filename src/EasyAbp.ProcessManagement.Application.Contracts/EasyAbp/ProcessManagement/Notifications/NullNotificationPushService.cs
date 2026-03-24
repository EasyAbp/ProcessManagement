using System;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using Volo.Abp.DependencyInjection;

namespace EasyAbp.ProcessManagement.Notifications;

/// <summary>
/// Default no-op implementation. Replaced by SignalR-based implementation when the HttpApi module is loaded.
/// </summary>
[Dependency(TryRegister = true)]
public class NullNotificationPushService : INotificationPushService, ITransientDependency
{
    public Task PushNewNotificationAsync(Guid userId, NotificationDto notification)
    {
        return Task.CompletedTask;
    }
}
