using System;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications;
using EasyAbp.ProcessManagement.Processes;
using Shouldly;
using Volo.Abp.Guids;
using Xunit;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.Notifications;

public class NotificationRepositoryTests : ProcessManagementEntityFrameworkCoreTestBase
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IGuidGenerator _guidGenerator;

    public NotificationRepositoryTests()
    {
        _notificationRepository = GetRequiredService<INotificationRepository>();
        _guidGenerator = GetRequiredService<IGuidGenerator>();
    }

    /// <summary>
    /// The set-based "dismiss all" must dismiss only the current user's not-yet-dismissed
    /// notifications, leaving other users' notifications and already-dismissed ones untouched.
    /// </summary>
    [Fact]
    public async Task DismissAllAsync_Should_Dismiss_Only_Not_Dismissed_Notifications_Of_The_User()
    {
        // Arrange
        var userA = _guidGenerator.Create();
        var userB = _guidGenerator.Create();
        var alreadyDismissedTime = new DateTime(2020, 1, 1);
        var dismissTime = new DateTime(2026, 6, 30, 12, 0, 0);

        var process = NewProcess();

        var userANormal1 = new Notification(_guidGenerator.Create(), process, userA);
        var userANormal2 = new Notification(_guidGenerator.Create(), process, userA);
        var userAAlreadyDismissed = new Notification(_guidGenerator.Create(), process, userA);
        userAAlreadyDismissed.SetDismissed(alreadyDismissedTime);
        var userBNotification = new Notification(_guidGenerator.Create(), process, userB);

        await WithUnitOfWorkAsync(() => _notificationRepository.InsertManyAsync(
            [userANormal1, userANormal2, userAAlreadyDismissed, userBNotification], autoSave: true));

        // Act
        await WithUnitOfWorkAsync(() =>
            _notificationRepository.DismissAllAsync(userA, DateTime.Now.AddYears(10), dismissTime));

        // Assert
        await WithUnitOfWorkAsync(async () =>
        {
            (await _notificationRepository.GetAsync(userANormal1.Id)).DismissedTime.ShouldBe(dismissTime);
            (await _notificationRepository.GetAsync(userANormal2.Id)).DismissedTime.ShouldBe(dismissTime);

            // Already-dismissed notification keeps its original time (not re-touched).
            (await _notificationRepository.GetAsync(userAAlreadyDismissed.Id)).DismissedTime
                .ShouldBe(alreadyDismissedTime);

            // Another user's notification stays untouched.
            (await _notificationRepository.GetAsync(userBNotification.Id)).DismissedTime.ShouldBeNull();
        });
    }

    /// <summary>
    /// The set-based dismiss-by-process must dismiss only the targeted process's not-yet-dismissed
    /// notifications, leaving other processes and already-dismissed ones untouched.
    /// </summary>
    [Fact]
    public async Task DismissByProcessIdAsync_Should_Dismiss_Only_Not_Dismissed_Notifications_Of_The_Process()
    {
        // Arrange
        var userId = _guidGenerator.Create();
        var alreadyDismissedTime = new DateTime(2020, 1, 1);
        var dismissTime = new DateTime(2026, 6, 30, 12, 0, 0);

        var targetProcess = NewProcess();
        var otherProcess = NewProcess();

        var targetNormal = new Notification(_guidGenerator.Create(), targetProcess, userId);
        var targetAlreadyDismissed = new Notification(_guidGenerator.Create(), targetProcess, userId);
        targetAlreadyDismissed.SetDismissed(alreadyDismissedTime);
        var otherProcessNotification = new Notification(_guidGenerator.Create(), otherProcess, userId);

        await WithUnitOfWorkAsync(() => _notificationRepository.InsertManyAsync(
            [targetNormal, targetAlreadyDismissed, otherProcessNotification], autoSave: true));

        // Act
        await WithUnitOfWorkAsync(() =>
            _notificationRepository.DismissByProcessIdAsync(targetProcess.Id, dismissTime));

        // Assert
        await WithUnitOfWorkAsync(async () =>
        {
            (await _notificationRepository.GetAsync(targetNormal.Id)).DismissedTime.ShouldBe(dismissTime);

            (await _notificationRepository.GetAsync(targetAlreadyDismissed.Id)).DismissedTime
                .ShouldBe(alreadyDismissedTime);

            (await _notificationRepository.GetAsync(otherProcessNotification.Id)).DismissedTime.ShouldBeNull();
        });
    }

    private ProcessEto NewProcess()
    {
        return new ProcessEto
        {
            Id = _guidGenerator.Create(),
            ProcessName = "FakeExport",
            CorrelationId = "test",
            GroupKey = "test",
            StateName = "Ready",
            StateFlag = ProcessStateFlag.Information,
            StateUpdateTime = DateTime.Now
        };
    }
}
