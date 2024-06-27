using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.ProcessManagement.Notifications;

public interface INotificationRepository : IRepository<Notification, Guid>
{
}
