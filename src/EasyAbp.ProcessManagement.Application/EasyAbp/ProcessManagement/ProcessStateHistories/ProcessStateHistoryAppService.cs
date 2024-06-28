using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Permissions;
using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.ProcessStateHistories.Dtos;
using EasyAbp.ProcessManagement.UserGroups;
using Microsoft.AspNetCore.Authorization;
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
            var groupKeys = await GetUserGroupKeys();

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
            var groupKeys = await GetUserGroupKeys();

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
            .WhereIf(!input.StateName.IsNullOrWhiteSpace(), x => x.StateName.Contains(input.StateName))
            .WhereIf(!input.ActionName.IsNullOrWhiteSpace(), x => x.ActionName.Contains(input.ActionName))
            .WhereIf(input.StateFlag != null, x => x.StateFlag == input.StateFlag)
            .WhereIf(!input.StateSummaryText.IsNullOrWhiteSpace(),
                x => x.StateSummaryText.Contains(input.StateSummaryText))
            .WhereIf(!input.StateDetailsText.IsNullOrWhiteSpace(),
                x => x.StateDetailsText.Contains(input.StateDetailsText))
            .WhereIf(input.StateUpdateTime != null, x => x.StateUpdateTime == input.StateUpdateTime)
            ;
    }

    protected override IQueryable<ProcessStateHistory> ApplyDefaultSorting(IQueryable<ProcessStateHistory> query)
    {
        return query.OrderBy(x => x.StateUpdateTime);
    }

    protected virtual async Task<bool> HasManagementPermissionAsync()
    {
        return await AuthorizationService.IsGrantedAsync(ProcessManagementPermissions.Process.Manage);
    }

    protected virtual async Task<List<string>> GetUserGroupKeys()
    {
        return await UserGroupManager.GetUserGroupKeysAsync(CurrentUser.GetId());
    }
}