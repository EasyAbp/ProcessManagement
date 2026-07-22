using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using EasyAbp.ProcessManagement.Processes;
using Shouldly;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using Xunit;

namespace EasyAbp.ProcessManagement.Notifications;

public class NotificationAppServiceTests : ProcessManagementApplicationTestBase
{
    private readonly INotificationAppService _notificationAppService;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IObjectMapper _objectMapper;

    public NotificationAppServiceTests()
    {
        _notificationAppService = GetRequiredService<INotificationAppService>();
        _notificationRepository = GetRequiredService<INotificationRepository>();
        _currentUser = GetRequiredService<ICurrentUser>();
        _guidGenerator = GetRequiredService<IGuidGenerator>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public void Should_Map_Notification_To_NotificationDto()
    {
        // Arrange
        var process = new ProcessEto
        {
            Id = _guidGenerator.Create(),
            ProcessName = "FakeExport",
            CorrelationId = _guidGenerator.Create().ToString(),
            GroupKey = "test",
            StateName = "Ready",
            ActionName = "Start",
            StateFlag = ProcessStateFlag.Success,
            StateSummaryText = "All good",
            StateUpdateTime = DateTime.Now
        };
        var userId = _guidGenerator.Create();
        var notification = new Notification(_guidGenerator.Create(), process, userId);

        // Act
        var dto = _objectMapper.Map<Notification, NotificationDto>(notification);

        // Assert
        dto.Id.ShouldBe(notification.Id);
        dto.UserId.ShouldBe(userId);
        dto.ProcessId.ShouldBe(process.Id);
        dto.ProcessName.ShouldBe(process.ProcessName);
        dto.CorrelationId.ShouldBe(process.CorrelationId);
        dto.GroupKey.ShouldBe(process.GroupKey);
        dto.StateName.ShouldBe(process.StateName);
        dto.ActionName.ShouldBe(process.ActionName);
        dto.StateFlag.ShouldBe(process.StateFlag);
        dto.StateSummaryText.ShouldBe(process.StateSummaryText);
        dto.StateUpdateTime.ShouldBe(process.StateUpdateTime);

        // Ignored, populated outside the map.
        dto.ProcessDisplayName.ShouldBeNull();
        dto.StateDisplayName.ShouldBeNull();
    }

    [Fact]
    public async Task Dismiss_By_Ids_Should_Dismiss_Only_The_Specified_Notifications()
    {
        // Arrange
        var notifications = await CreateNotificationsForCurrentUserAsync(3);
        var toDismiss = notifications.Take(2).Select(x => x.Id).ToList();

        // Act
        await _notificationAppService.DismissAsync(new DismissNotificationDto { NotificationIds = toDismiss });

        // Assert
        await WithUnitOfWorkAsync(async () =>
        {
            (await _notificationRepository.GetAsync(notifications[0].Id)).DismissedTime.ShouldNotBeNull();
            (await _notificationRepository.GetAsync(notifications[1].Id)).DismissedTime.ShouldNotBeNull();
            (await _notificationRepository.GetAsync(notifications[2].Id)).DismissedTime.ShouldBeNull();
        });
    }

    [Fact]
    public async Task Dismiss_By_MaxCreationTime_Should_Dismiss_All_Of_The_Users_Notifications()
    {
        // Arrange
        var notifications = await CreateNotificationsForCurrentUserAsync(5);

        // Act
        await _notificationAppService.DismissAsync(
            new DismissNotificationDto { MaxCreationTime = DateTime.Now.AddYears(10) });

        // Assert
        await WithUnitOfWorkAsync(async () =>
        {
            foreach (var notification in notifications)
            {
                (await _notificationRepository.GetAsync(notification.Id)).DismissedTime.ShouldNotBeNull();
            }
        });
    }

    [Fact]
    public async Task ReadAsync_Should_Set_The_Read_Time()
    {
        // Arrange
        var notifications = await CreateNotificationsForCurrentUserAsync(1);

        // Act
        await _notificationAppService.ReadAsync(notifications[0].Id);

        // Assert
        await WithUnitOfWorkAsync(async () =>
        {
            (await _notificationRepository.GetAsync(notifications[0].Id)).ReadTime.ShouldNotBeNull();
        });
    }

    [Fact]
    public async Task GetListAsync_Should_Return_The_Current_Users_Notifications()
    {
        // Arrange
        await CreateNotificationsForCurrentUserAsync(3);

        // Act
        var result = await _notificationAppService.GetListAsync(new NotificationGetListInput
        {
            UserId = _currentUser.GetId()
        });

        // Assert
        result.TotalCount.ShouldBe(3);
        result.Items.ShouldAllBe(x => x.UserId == _currentUser.GetId());
    }

    private async Task<List<Notification>> CreateNotificationsForCurrentUserAsync(int count)
    {
        var userId = _currentUser.GetId();

        var process = new ProcessEto
        {
            Id = _guidGenerator.Create(),
            ProcessName = "FakeExport",
            CorrelationId = _guidGenerator.Create().ToString(),
            GroupKey = "test",
            StateName = "Ready",
            StateFlag = ProcessStateFlag.Information,
            StateUpdateTime = DateTime.Now
        };

        var notifications = Enumerable.Range(0, count)
            .Select(_ => new Notification(_guidGenerator.Create(), process, userId))
            .ToList();

        await WithUnitOfWorkAsync(() => _notificationRepository.InsertManyAsync(notifications, autoSave: true));

        return notifications;
    }
}
