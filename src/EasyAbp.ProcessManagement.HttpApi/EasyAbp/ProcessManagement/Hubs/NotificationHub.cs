using EasyAbp.ProcessManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.SignalR;

namespace EasyAbp.ProcessManagement.Hubs;

[Authorize(ProcessManagementPermissions.Process.Default)]
[HubRoute("/signalr-hubs/process-management/notification")]
public class NotificationHub : AbpHub
{
}
