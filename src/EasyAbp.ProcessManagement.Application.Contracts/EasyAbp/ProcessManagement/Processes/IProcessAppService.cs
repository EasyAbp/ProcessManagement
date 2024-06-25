using System;
using EasyAbp.ProcessManagement.Processes.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.ProcessManagement.Processes;

public interface IProcessAppService :
    IReadOnlyAppService<
        ProcessDto,
        Guid,
        ProcessGetListInput>
{
}