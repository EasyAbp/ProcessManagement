using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.SignalR;

namespace EasyAbp.ProcessManagement.Hubs;

[Authorize]
[HubRoute("/signalr-hubs/process-management/notification")]
public class NotificationHub : AbpHub
{
}
