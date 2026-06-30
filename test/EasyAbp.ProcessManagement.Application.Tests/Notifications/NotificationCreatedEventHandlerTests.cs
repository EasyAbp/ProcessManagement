using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using EasyAbp.ProcessManagement.Processes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Xunit;

namespace EasyAbp.ProcessManagement.Notifications;

/// <summary>
/// A counting fake replacing <see cref="INotificationPushService"/> so the test can assert
/// how many times <see cref="NotificationCreatedEventHandler"/> actually executes.
/// </summary>
public class CountingNotificationPushService : INotificationPushService
{
    public List<Guid> PushedUserIds { get; } = [];

    public Task PushNewNotificationAsync(Guid userId, NotificationDto notification)
    {
        PushedUserIds.Add(userId);
        return Task.CompletedTask;
    }
}

[DependsOn(typeof(ProcessManagementApplicationTestModule))]
public class NotificationEventTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(
            ServiceDescriptor.Singleton<INotificationPushService, CountingNotificationPushService>());
    }
}

public class NotificationCreatedEventHandlerTests : ProcessManagementTestBase<NotificationEventTestModule>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly CountingNotificationPushService _pushService;

    public NotificationCreatedEventHandlerTests()
    {
        _notificationRepository = GetRequiredService<INotificationRepository>();
        _guidGenerator = GetRequiredService<IGuidGenerator>();
        _pushService = (CountingNotificationPushService)GetRequiredService<INotificationPushService>();
    }

    /// <summary>
    /// Proves the batched insert introduced in <see cref="ProcessChangedEventHandler"/> does NOT
    /// swallow the per-entity created event: a single <c>InsertManyAsync</c> of N notifications still
    /// raises <c>EntityCreatedEventData&lt;Notification&gt;</c> N times, so the
    /// <see cref="NotificationCreatedEventHandler"/> (and thus the SignalR push) runs once per notification.
    /// </summary>
    [Fact]
    public async Task InsertManyAsync_Should_Trigger_NotificationCreatedEventHandler_Once_Per_Entity()
    {
        // Arrange
        const int count = 50;
        var notifications = BuildNotifications(count, out var expectedUserIds);

        // Act - this is exactly the call ProcessChangedEventHandler now makes.
        await WithUnitOfWorkAsync(async () =>
        {
            await _notificationRepository.InsertManyAsync(notifications, autoSave: true);
        });

        // Assert - the push handler ran for every single notification.
        _pushService.PushedUserIds.Count.ShouldBe(count);
        _pushService.PushedUserIds.ShouldBe(expectedUserIds, ignoreOrder: true);
    }

    private List<Notification> BuildNotifications(int count, out List<Guid> userIds)
    {
        var process = new ProcessEto
        {
            Id = _guidGenerator.Create(),
            ProcessName = "FakeExport",
            CorrelationId = "test",
            GroupKey = "test",
            StateName = "Ready",
            StateFlag = ProcessStateFlag.Information,
            StateUpdateTime = DateTime.Now
        };

        var notifications = new List<Notification>();
        userIds = [];

        for (var i = 0; i < count; i++)
        {
            var userId = _guidGenerator.Create();
            userIds.Add(userId);
            notifications.Add(new Notification(_guidGenerator.Create(), process, userId));
        }

        return notifications;
    }
}
