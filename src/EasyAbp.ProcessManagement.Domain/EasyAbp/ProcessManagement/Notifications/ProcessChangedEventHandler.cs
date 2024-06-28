using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.UserGroups;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace EasyAbp.ProcessManagement.Notifications;

public class ProcessChangedEventHandler : ITransientDependency,
    IDistributedEventHandler<EntityCreatedEto<ProcessEto>>,
    IDistributedEventHandler<EntityUpdatedEto<ProcessEto>>
{
    private readonly IClock _clock;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IUserGroupManager _userGroupManager;
    private readonly INotificationRepository _notificationRepository;

    public ProcessChangedEventHandler(
        IClock clock,
        IGuidGenerator guidGenerator,
        IUserGroupManager userGroupManager,
        INotificationRepository notificationRepository)
    {
        _clock = clock;
        _guidGenerator = guidGenerator;
        _userGroupManager = userGroupManager;
        _notificationRepository = notificationRepository;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityCreatedEto<ProcessEto> eventData)
    {
        var userIds = await _userGroupManager.GetUserIdsAsync(eventData.Entity.GroupKey);

        foreach (var userId in userIds)
        {
            await _notificationRepository.InsertAsync(
                new Notification(_guidGenerator.Create(), eventData.Entity, userId), true);
        }
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityUpdatedEto<ProcessEto> eventData)
    {
        var now = _clock.Now;
        var userIds = await _userGroupManager.GetUserIdsAsync(eventData.Entity.GroupKey);

        var oldNotifications = await _notificationRepository.GetListAsync(x => x.ProcessId == eventData.Entity.Id);

        foreach (var oldNotification in oldNotifications)
        {
            if (oldNotification.DismissedTime.HasValue)
            {
                continue;
            }

            oldNotification.SetDismissed(now);
            await _notificationRepository.UpdateAsync(oldNotification);
        }

        foreach (var userId in userIds)
        {
            await _notificationRepository.InsertAsync(
                new Notification(_guidGenerator.Create(), eventData.Entity, userId), true);
        }
    }
}