using System;
using EasyAbp.ProcessManagement.ProcessStateHistories.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public interface IProcessStateHistoryAppService :
    IReadOnlyAppService<
        ProcessStateHistoryDto,
        Guid,
        ProcessStateHistoryGetListInput>
{
}