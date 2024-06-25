using System;
using EasyAbp.ProcessManagement.ProcessStateHistories.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

[RemoteService(Name = ProcessManagementRemoteServiceConsts.RemoteServiceName)]
[Route("/api/process-management/process-state-history")]
public class ProcessStateHistoryController : ProcessManagementController, IProcessStateHistoryAppService
{
    private readonly IProcessStateHistoryAppService _service;

    public ProcessStateHistoryController(IProcessStateHistoryAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{id}")]
        public virtual Task<ProcessStateHistoryDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    [Route("")]
        public virtual Task<PagedResultDto<ProcessStateHistoryDto>> GetListAsync(ProcessStateHistoryGetListInput input)
    {
        return _service.GetListAsync(input);
    }
}