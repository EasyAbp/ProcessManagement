using System;
using System.Collections.Generic;

namespace EasyAbp.ProcessManagement.Notifications.Dtos;

public class DismissNotificationDto
{
    public List<Guid> NotificationIds { get; set; }
}