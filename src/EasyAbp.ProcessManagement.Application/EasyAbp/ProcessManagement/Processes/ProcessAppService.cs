using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Permissions;
using EasyAbp.ProcessManagement.Processes.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.ProcessManagement.Processes;


public class ProcessAppService : ReadOnlyAppService<Process, ProcessDto, Guid, ProcessGetListInput>,
    IProcessAppService
{
    protected override string GetPolicyName { get; set; } = ProcessManagementPermissions.Process.Default;
    protected override string GetListPolicyName { get; set; } = ProcessManagementPermissions.Process.Default;

    private readonly IProcessRepository _repository;

    public ProcessAppService(IProcessRepository repository) : base(repository)
    {
        _repository = repository;
    }

    protected override async Task<IQueryable<Process>> CreateFilteredQueryAsync(ProcessGetListInput input)
    {
        // TODO: AbpHelper generated
        return (await base.CreateFilteredQueryAsync(input))
            .WhereIf(!input.ProcessName.IsNullOrWhiteSpace(), x => x.ProcessName.Contains(input.ProcessName))
            .WhereIf(!input.CorrelationId.IsNullOrWhiteSpace(), x => x.CorrelationId.Contains(input.CorrelationId))
            .WhereIf(!input.GroupKey.IsNullOrWhiteSpace(), x => x.GroupKey.Contains(input.GroupKey))
            .WhereIf(input.CompletionTime != null, x => x.CompletionTime == input.CompletionTime)
            .WhereIf(input.StateUpdateTime != null, x => x.StateUpdateTime == input.StateUpdateTime)
            .WhereIf(!input.StateName.IsNullOrWhiteSpace(), x => x.StateName.Contains(input.StateName))
            .WhereIf(!input.SubStateName.IsNullOrWhiteSpace(), x => x.SubStateName.Contains(input.SubStateName))
            .WhereIf(input.StateFlag != null, x => x.StateFlag == input.StateFlag)
            .WhereIf(!input.StateSummaryText.IsNullOrWhiteSpace(), x => x.StateSummaryText.Contains(input.StateSummaryText))
            .WhereIf(!input.StateDetailsText.IsNullOrWhiteSpace(), x => x.StateDetailsText.Contains(input.StateDetailsText))
            ;
    }
}
