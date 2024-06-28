using System;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.ProcessManagement.Notifications;

[RemoteService(Name = ProcessManagementRemoteServiceConsts.RemoteServiceName)]
[Route("/api/process-management/notification")]
public class NotificationController : ProcessManagementController, INotificationAppService
{
    private readonly INotificationAppService _service;

    public NotificationController(INotificationAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<NotificationDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    [Route("")]
    public virtual Task<PagedResultDto<NotificationDto>> GetListAsync(NotificationGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPost]
    [Route("read")]
    public virtual Task ReadAsync(Guid id)
    {
        return _service.ReadAsync(id);
    }

    [HttpPost]
    [Route("dismiss")]
    public virtual Task DismissAsync(DismissNotificationDto input)
    {
        return _service.DismissAsync(input);
    }
}