using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Permissions;
using EasyAbp.ProcessManagement.ProcessStateHistories.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public class ProcessStateHistoryAppService : ReadOnlyAppService<ProcessStateHistory, ProcessStateHistoryDto, Guid,
        ProcessStateHistoryGetListInput>,
    IProcessStateHistoryAppService
{
    protected override string GetPolicyName { get; set; } = ProcessManagementPermissions.Process.Default;
    protected override string GetListPolicyName { get; set; } = ProcessManagementPermissions.Process.Default;

    private readonly IProcessStateHistoryRepository _repository;

    public ProcessStateHistoryAppService(IProcessStateHistoryRepository repository) : base(repository)
    {
        _repository = repository;
    }

    protected override async Task<IQueryable<ProcessStateHistory>> CreateFilteredQueryAsync(
        ProcessStateHistoryGetListInput input)
    {
        // TODO: AbpHelper generated
        return (await base.CreateFilteredQueryAsync(input))
            .WhereIf(input.ProcessId != null, x => x.ProcessId == input.ProcessId)
            .WhereIf(!input.StateName.IsNullOrWhiteSpace(), x => x.StateName.Contains(input.StateName))
            .WhereIf(!input.SubStateName.IsNullOrWhiteSpace(), x => x.SubStateName.Contains(input.SubStateName))
            .WhereIf(input.StateFlag != null, x => x.StateFlag == input.StateFlag)
            .WhereIf(!input.StateSummaryText.IsNullOrWhiteSpace(),
                x => x.StateSummaryText.Contains(input.StateSummaryText))
            .WhereIf(!input.StateDetailsText.IsNullOrWhiteSpace(),
                x => x.StateDetailsText.Contains(input.StateDetailsText))
            .WhereIf(input.StateUpdateTime != null, x => x.StateUpdateTime == input.StateUpdateTime)
            ;
    }
}