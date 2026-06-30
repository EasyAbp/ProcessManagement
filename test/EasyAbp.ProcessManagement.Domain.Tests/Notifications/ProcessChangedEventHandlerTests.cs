using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.UserGroups;
using Shouldly;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Xunit;

namespace EasyAbp.ProcessManagement.Notifications;

public class ProcessChangedEventHandlerTests : ProcessManagementDomainTestBase
{
    private readonly ProcessChangedEventHandler _handler;
    private readonly INotificationRepository _notificationRepository;

    public ProcessChangedEventHandlerTests()
    {
        _handler = GetRequiredService<ProcessChangedEventHandler>();
        _notificationRepository = GetRequiredService<INotificationRepository>();
    }

    [Fact]
    public async Task Should_Create_A_Notification_For_Each_Group_User_On_Process_Created()
    {
        // Arrange
        var process = NewProcess(TestGroupUserGroupContributor.GroupKey);

        // Act
        await _handler.HandleEventAsync(new EntityCreatedEto<ProcessEto>(process));

        // Assert - one notification per group user, none dismissed.
        await WithUnitOfWorkAsync(async () =>
        {
            var notifications = await _notificationRepository.GetListAsync(x => x.ProcessId == process.Id);

            notifications.Count.ShouldBe(TestGroupUserGroupContributor.UserIds.Count);
            notifications.Select(x => x.UserId)
                .ShouldBe(TestGroupUserGroupContributor.UserIds, ignoreOrder: true);
            notifications.ShouldAllBe(x => x.DismissedTime == null);
        });
    }

    [Fact]
    public async Task Should_Dismiss_Existing_Notifications_And_Create_New_On_Process_Updated()
    {
        // Arrange - a process that already has a notification per group user.
        var process = NewProcess(TestGroupUserGroupContributor.GroupKey);
        await _handler.HandleEventAsync(new EntityCreatedEto<ProcessEto>(process));

        // Act - an update dismisses the existing notifications and creates fresh ones.
        await _handler.HandleEventAsync(new EntityUpdatedEto<ProcessEto>(process));

        // Assert
        await WithUnitOfWorkAsync(async () =>
        {
            var userCount = TestGroupUserGroupContributor.UserIds.Count;
            var all = await _notificationRepository.GetListAsync(x => x.ProcessId == process.Id);

            all.Count.ShouldBe(userCount * 2);
            all.Count(x => x.DismissedTime != null).ShouldBe(userCount); // old ones dismissed
            all.Count(x => x.DismissedTime == null).ShouldBe(userCount); // new ones active
        });
    }

    [Fact]
    public async Task Should_Not_Re_Dismiss_Already_Dismissed_Notifications_On_Update()
    {
        // Arrange - create, then update twice. The first update dismisses the original set;
        // the second update must not touch those already-dismissed notifications again.
        var process = NewProcess(TestGroupUserGroupContributor.GroupKey);
        await _handler.HandleEventAsync(new EntityCreatedEto<ProcessEto>(process));
        await _handler.HandleEventAsync(new EntityUpdatedEto<ProcessEto>(process));
        await _handler.HandleEventAsync(new EntityUpdatedEto<ProcessEto>(process));

        // Assert - 3 generations of notifications; only the latest generation stays active.
        await WithUnitOfWorkAsync(async () =>
        {
            var userCount = TestGroupUserGroupContributor.UserIds.Count;
            var all = await _notificationRepository.GetListAsync(x => x.ProcessId == process.Id);

            all.Count.ShouldBe(userCount * 3);
            all.Count(x => x.DismissedTime == null).ShouldBe(userCount);
        });
    }

    private static ProcessEto NewProcess(string groupKey)
    {
        return new ProcessEto
        {
            Id = Guid.NewGuid(),
            ProcessName = "FakeExport",
            CorrelationId = Guid.NewGuid().ToString(),
            GroupKey = groupKey,
            StateName = "Ready",
            StateFlag = ProcessStateFlag.Information,
            StateUpdateTime = DateTime.Now
        };
    }
}
