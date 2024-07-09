using System;
using System.Collections.Generic;

namespace EasyAbp.ProcessManagement.Web.Options;

public class ProcessManagementWebOptions
{
    public TimeSpan NotificationLifetime { get; set; } = TimeSpan.FromDays(7);

    /// <summary>
    /// Defined actions for users to initiate.
    /// </summary>
    public List<ProcessStateActionDefinition> Actions { get; } = new();
}