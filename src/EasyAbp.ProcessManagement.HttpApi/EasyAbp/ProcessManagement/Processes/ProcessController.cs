using System;
using EasyAbp.ProcessManagement.Processes.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.ProcessManagement.Processes;

[RemoteService(Name = ProcessManagementRemoteServiceConsts.RemoteServiceName)]
[Route("/api/process-management/process")]
public class ProcessController : ProcessManagementController, IProcessAppService
{
    private readonly IProcessAppService _service;

    public ProcessController(IProcessAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{id}")]
        public virtual Task<ProcessDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    [Route("")]
        public virtual Task<PagedResultDto<ProcessDto>> GetListAsync(ProcessGetListInput input)
    {
        return _service.GetListAsync(input);
    }
}