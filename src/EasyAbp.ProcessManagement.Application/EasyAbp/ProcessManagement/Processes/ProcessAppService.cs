using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Options;
using EasyAbp.ProcessManagement.Permissions;
using EasyAbp.ProcessManagement.Processes.Dtos;
using EasyAbp.ProcessManagement.UserGroups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Users;

namespace EasyAbp.ProcessManagement.Processes;

public class ProcessAppService : ReadOnlyAppService<Process, ProcessDto, Guid, ProcessGetListInput>,
    IProcessAppService
{
    protected override string GetPolicyName { get; set; } = ProcessManagementPermissions.Process.Default;
    protected override string GetListPolicyName { get; set; } = ProcessManagementPermissions.Process.Default;

    protected IUserGroupManager UserGroupManager => LazyServiceProvider.LazyGetRequiredService<IUserGroupManager>();

    public ProcessAppService(IProcessRepository repository) : base(repository)
    {
    }

    public override async Task<ProcessDto> GetAsync(Guid id)
    {
        await CheckGetPolicyAsync();

        var entity = await GetEntityByIdAsync(id);

        if (!await HasManagementPermissionAsync())
        {
            var groupKeys = await GetUserGroupKeys(CurrentUser.GetId());

            if (!groupKeys.Contains(entity.GroupKey))
            {
                throw new AbpAuthorizationException();
            }
        }

        return await MapToGetOutputDtoAsync(entity);
    }

    protected override async Task<IQueryable<Process>> CreateFilteredQueryAsync(ProcessGetListInput input)
    {
        var queryable = await base.CreateFilteredQueryAsync(input);

        var hasUserNameInput = !input.UserName.IsNullOrWhiteSpace();

        if (hasUserNameInput && input.UserName != CurrentUser.UserName && !await HasManagementPermissionAsync())
        {
            throw new AbpAuthorizationException();
        }

        if (hasUserNameInput)
        {
            var groupKeys = await GetUserGroupKeys(CurrentUser.GetId());
            queryable = queryable.Where(x => groupKeys.Contains(x.GroupKey));
        }

        return queryable
                .WhereIf(!input.GroupKey.IsNullOrWhiteSpace(), x => x.GroupKey.Contains(input.GroupKey))
                .WhereIf(!input.ProcessName.IsNullOrWhiteSpace(), x => x.ProcessName.Contains(input.ProcessName))
                .WhereIf(!input.CorrelationId.IsNullOrWhiteSpace(), x => x.CorrelationId.Contains(input.CorrelationId))
                .WhereIf(input.StateUpdateTime != null, x => x.StateUpdateTime == input.StateUpdateTime)
                .WhereIf(!input.StateName.IsNullOrWhiteSpace(), x => x.StateName.Contains(input.StateName))
                .WhereIf(!input.ActionName.IsNullOrWhiteSpace(), x => x.ActionName.Contains(input.ActionName))
                .WhereIf(input.StateFlag != null, x => x.StateFlag == input.StateFlag)
                .WhereIf(!input.StateSummaryText.IsNullOrWhiteSpace(),
                    x => x.StateSummaryText.Contains(input.StateSummaryText))
            ;
    }

    protected virtual async Task<bool> HasManagementPermissionAsync()
    {
        return await AuthorizationService.IsGrantedAsync(ProcessManagementPermissions.Process.Manage);
    }

    protected virtual async Task<List<string>> GetUserGroupKeys(Guid userId)
    {
        return await UserGroupManager.GetUserGroupKeysAsync(userId);
    }

    protected override ProcessDto MapToGetOutputDto(Process entity)
    {
        var options = LazyServiceProvider.LazyGetRequiredService<IOptions<ProcessManagementOptions>>();

        var processDefinition = options.Value.GetProcessDefinition(entity.ProcessName);
        var stateDefinition = processDefinition.GetState(entity.StateName);

        var dto = base.MapToGetOutputDto(entity);

        dto.ProcessDisplayName = processDefinition.DisplayName?.Localize(StringLocalizerFactory) ?? dto.ProcessName;
        dto.StateDisplayName = stateDefinition.DisplayName?.Localize(StringLocalizerFactory) ?? dto.StateName;

        return dto;
    }

    protected override IQueryable<Process> ApplyDefaultSorting(IQueryable<Process> query)
    {
        return query.OrderByDescending(e => e.Id);
    }

    protected override ProcessDto MapToGetListOutputDto(Process entity)
    {
        return MapToGetOutputDto(entity);
    }
}