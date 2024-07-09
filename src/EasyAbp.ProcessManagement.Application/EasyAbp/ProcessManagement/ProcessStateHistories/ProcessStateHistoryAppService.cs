using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Options;
using EasyAbp.ProcessManagement.Permissions;
using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.ProcessStateHistories.Dtos;
using EasyAbp.ProcessManagement.UserGroups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Users;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public class ProcessStateHistoryAppService : ReadOnlyAppService<ProcessStateHistory, ProcessStateHistoryDto, Guid,
    ProcessStateHistoryGetListInput>, IProcessStateHistoryAppService
{
    protected override string GetPolicyName { get; set; } = ProcessManagementPermissions.Process.Default;
    protected override string GetListPolicyName { get; set; } = ProcessManagementPermissions.Process.Default;

    protected IUserGroupManager UserGroupManager => LazyServiceProvider.LazyGetRequiredService<IUserGroupManager>();
    protected IProcessRepository ProcessRepository => LazyServiceProvider.LazyGetRequiredService<IProcessRepository>();

    public ProcessStateHistoryAppService(IProcessStateHistoryRepository repository) : base(repository)
    {
    }

    public override async Task<ProcessStateHistoryDto> GetAsync(Guid id)
    {
        await CheckGetPolicyAsync();

        var history = await GetEntityByIdAsync(id);

        if (!await HasManagementPermissionAsync())
        {
            var process = await ProcessRepository.GetAsync(history.ProcessId);
            var groupKeys = await GetUserGroupKeys(CurrentUser.GetId());

            if (!groupKeys.Contains(process.GroupKey))
            {
                throw new AbpAuthorizationException();
            }
        }

        return await MapToGetOutputDtoAsync(history);
    }

    public override async Task<PagedResultDto<ProcessStateHistoryDto>> GetListAsync(
        ProcessStateHistoryGetListInput input)
    {
        await CheckGetListPolicyAsync();

        if (!await HasManagementPermissionAsync())
        {
            var process = await ProcessRepository.GetAsync(input.ProcessId);
            var groupKeys = await GetUserGroupKeys(CurrentUser.GetId());

            if (!groupKeys.Contains(process.GroupKey))
            {
                throw new AbpAuthorizationException();
            }
        }

        var query = await CreateFilteredQueryAsync(input);
        var totalCount = await AsyncExecuter.CountAsync(query);

        var entityDtos = new List<ProcessStateHistoryDto>();

        if (totalCount > 0)
        {
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);
            entityDtos = await MapToGetListOutputDtosAsync(entities);
        }

        return new PagedResultDto<ProcessStateHistoryDto>(
            totalCount,
            entityDtos
        );
    }

    protected override async Task<IQueryable<ProcessStateHistory>> CreateFilteredQueryAsync(
        ProcessStateHistoryGetListInput input)
    {
        return (await base.CreateFilteredQueryAsync(input))
            .Where(x => x.ProcessId == input.ProcessId)
            .WhereIf(!input.ProcessName.IsNullOrWhiteSpace(), x => x.ProcessName.Contains(input.ProcessName))
            .WhereIf(!input.StateName.IsNullOrWhiteSpace(), x => x.StateName.Contains(input.StateName))
            .WhereIf(!input.ActionName.IsNullOrWhiteSpace(), x => x.ActionName.Contains(input.ActionName))
            .WhereIf(input.StateFlag != null, x => x.StateFlag == input.StateFlag)
            .WhereIf(!input.StateSummaryText.IsNullOrWhiteSpace(),
                x => x.StateSummaryText.Contains(input.StateSummaryText))
            .WhereIf(input.StateUpdateTime != null, x => x.StateUpdateTime == input.StateUpdateTime)
            ;
    }

    protected override IQueryable<ProcessStateHistory> ApplyDefaultSorting(IQueryable<ProcessStateHistory> query)
    {
        return query.OrderBy(x => x.StateUpdateTime);
    }

    protected override ProcessStateHistoryDto MapToGetOutputDto(ProcessStateHistory entity)
    {
        var options = LazyServiceProvider.LazyGetRequiredService<IOptions<ProcessManagementOptions>>();

        var processDefinition = options.Value.GetProcessDefinition(entity.ProcessName);
        var stateDefinition = processDefinition.GetState(entity.StateName);

        var dto = base.MapToGetOutputDto(entity);

        dto.ProcessDisplayName = processDefinition.DisplayName?.Localize(StringLocalizerFactory) ?? dto.ProcessName;
        dto.StateDisplayName = stateDefinition.DisplayName?.Localize(StringLocalizerFactory) ?? dto.StateName;

        return dto;
    }

    protected override ProcessStateHistoryDto MapToGetListOutputDto(ProcessStateHistory entity)
    {
        return MapToGetOutputDto(entity);
    }

    protected virtual async Task<bool> HasManagementPermissionAsync()
    {
        return await AuthorizationService.IsGrantedAsync(ProcessManagementPermissions.Process.Manage);
    }

    protected virtual async Task<List<string>> GetUserGroupKeys(Guid userId)
    {
        return await UserGroupManager.GetUserGroupKeysAsync(userId);
    }
}