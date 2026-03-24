using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using EasyAbp.ProcessManagement.Options;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectMapping;

namespace EasyAbp.ProcessManagement.Notifications;

public class NotificationCreatedEventHandler : ILocalEventHandler<EntityCreatedEventData<Notification>>,
    ITransientDependency
{
    private readonly IObjectMapper<ProcessManagementApplicationModule> _objectMapper;
    private readonly IOptions<ProcessManagementOptions> _options;
    private readonly IStringLocalizerFactory _stringLocalizerFactory;
    private readonly INotificationPushService _notificationPushService;

    public NotificationCreatedEventHandler(
        IObjectMapper<ProcessManagementApplicationModule> objectMapper,
        IOptions<ProcessManagementOptions> options,
        IStringLocalizerFactory stringLocalizerFactory,
        INotificationPushService notificationPushService)
    {
        _objectMapper = objectMapper;
        _options = options;
        _stringLocalizerFactory = stringLocalizerFactory;
        _notificationPushService = notificationPushService;
    }

    public async Task HandleEventAsync(EntityCreatedEventData<Notification> eventData)
    {
        var notification = eventData.Entity;

        var processDefinition = _options.Value.GetProcessDefinition(notification.ProcessName);
        var stateDefinition = processDefinition.GetState(notification.StateName);

        var dto = _objectMapper.Map<Notification, NotificationDto>(notification);

        dto.ProcessDisplayName =
            processDefinition.DisplayName?.Localize(_stringLocalizerFactory) ?? notification.ProcessName;
        dto.StateDisplayName =
            stateDefinition.DisplayName?.Localize(_stringLocalizerFactory) ?? notification.StateName;

        await _notificationPushService.PushNewNotificationAsync(notification.UserId, dto);
    }
}
