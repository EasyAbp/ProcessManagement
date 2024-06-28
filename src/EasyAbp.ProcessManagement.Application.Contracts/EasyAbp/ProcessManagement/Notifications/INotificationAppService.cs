using System;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.ProcessManagement.Notifications;

public interface INotificationAppService : IReadOnlyAppService<NotificationDto, Guid, NotificationGetListInput>
{
    Task ReadAsync(Guid id);

    Task DismissAsync(DismissNotificationDto input);
}