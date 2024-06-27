using System;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.ProcessManagement.Notifications;

public interface INotificationAppService : IReadOnlyAppService<NotificationDto, Guid, NotificationGetListInput>
{
}