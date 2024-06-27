using System;

namespace EasyAbp.ProcessManagement.Web.Options;

public class ProcessManagementWebOptions
{
    public TimeSpan NotificationLifetime { get; set; } = TimeSpan.FromDays(7);
}