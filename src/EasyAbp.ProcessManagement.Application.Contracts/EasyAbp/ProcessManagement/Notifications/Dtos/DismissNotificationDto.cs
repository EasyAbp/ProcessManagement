using System;
using System.Collections.Generic;

namespace EasyAbp.ProcessManagement.Notifications.Dtos;

public class DismissNotificationDto
{
    public DateTime? MaxCreationTime { get; set; }

    public List<Guid>? NotificationIds { get; set; }
}