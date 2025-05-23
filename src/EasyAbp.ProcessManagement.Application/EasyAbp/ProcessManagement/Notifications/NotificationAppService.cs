using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using EasyAbp.ProcessManagement.Options;
using EasyAbp.ProcessManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace EasyAbp.ProcessManagement.Notifications;

[Authorize]
public class NotificationAppService : ReadOnlyAppService<Notification, NotificationDto, Guid, NotificationGetListInput>,
    INotificationAppService
{
    protected override string? GetPolicyName { get; set; } = ProcessManagementPermissions.Process.Default;
    protected override string? GetListPolicyName { get; set; } = ProcessManagementPermissions.Process.Default;

    private readonly INotificationRepository _repository;

    public NotificationAppService(INotificationRepository repository) : base(repository)
    {
        _repository = repository;
    }

    protected override async Task<IQueryable<Notification>> CreateFilteredQueryAsync(NotificationGetListInput input)
    {
        return (await base.CreateFilteredQueryAsync(input))
            .WhereIf(input.ProcessId != null, x => x.ProcessId == input.ProcessId)
            .WhereIf(input.FromCreationTime.HasValue, x => x.CreationTime >= input.FromCreationTime)
            .WhereIf(input.ToCreationTime.HasValue, x => x.CreationTime <= input.ToCreationTime)
            .WhereIf(input.UserId != null, x => x.UserId == input.UserId)
            .WhereIf(input.ReadTime.HasValue, x => x.ReadTime == input.ReadTime)
            .WhereIf(input.DismissedTime.HasValue, x => x.DismissedTime == input.DismissedTime)
            .WhereIf(input.Read == true, x => x.ReadTime != null)
            .WhereIf(input.Read == false, x => x.ReadTime == null)
            .WhereIf(input.Dismissed == true, x => x.DismissedTime != null)
            .WhereIf(input.Dismissed == false, x => x.DismissedTime == null)
            .WhereIf(!input.ProcessName.IsNullOrWhiteSpace(), x => x.ProcessName.Contains(input.ProcessName))
            .WhereIf(!input.CorrelationId.IsNullOrWhiteSpace(), x => x.CorrelationId.Contains(input.CorrelationId))
            .WhereIf(!input.GroupKey.IsNullOrWhiteSpace(), x => x.GroupKey.Contains(input.GroupKey))
            .WhereIf(input.StateUpdateTime != null, x => x.StateUpdateTime == input.StateUpdateTime)
            .WhereIf(!input.StateName.IsNullOrWhiteSpace(), x => x.StateName.Contains(input.StateName))
            .WhereIf(input.ActionName != null, x => x.ActionName == input.ActionName)
            .WhereIf(input.StateFlag != null, x => x.StateFlag == input.StateFlag)
            .WhereIf(input.StateSummaryText != null, x => x.StateSummaryText == input.StateSummaryText)
            ;
    }

    public override async Task<NotificationDto> GetAsync(Guid id)
    {
        await CheckGetPolicyAsync();

        var entity = await GetEntityByIdAsync(id);

        if (entity.UserId != CurrentUser.GetId())
        {
            await CheckPolicyAsync(ProcessManagementPermissions.Process.Manage);
        }

        return await MapToGetOutputDtoAsync(entity);
    }

    public override async Task<PagedResultDto<NotificationDto>> GetListAsync(NotificationGetListInput input)
    {
        await CheckGetListPolicyAsync();

        if (input.UserId != CurrentUser.GetId())
        {
            await CheckPolicyAsync(ProcessManagementPermissions.Process.Manage);
        }

        var query = await CreateFilteredQueryAsync(input);
        var totalCount = await AsyncExecuter.CountAsync(query);

        var entityDtos = new List<NotificationDto>();

        if (totalCount > 0)
        {
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);
            entityDtos = await MapToGetListOutputDtosAsync(entities);
        }

        return new PagedResultDto<NotificationDto>(
            totalCount,
            entityDtos
        );
    }

    protected override NotificationDto MapToGetOutputDto(Notification entity)
    {
        var options = LazyServiceProvider.LazyGetRequiredService<IOptions<ProcessManagementOptions>>();

        var processDefinition = options.Value.GetProcessDefinition(entity.ProcessName);
        var stateDefinition = processDefinition.GetState(entity.StateName);

        var dto = base.MapToGetOutputDto(entity);

        dto.ProcessDisplayName = processDefinition.DisplayName?.Localize(StringLocalizerFactory) ?? dto.ProcessName;
        dto.StateDisplayName = stateDefinition.DisplayName?.Localize(StringLocalizerFactory) ?? dto.StateName;

        return dto;
    }

    protected override IQueryable<Notification> ApplyDefaultSorting(IQueryable<Notification> query)
    {
        return query.OrderByDescending(e => e.Id);
    }

    protected override NotificationDto MapToGetListOutputDto(Notification entity)
    {
        return MapToGetOutputDto(entity);
    }

    public virtual async Task ReadAsync(Guid id)
    {
        var notification = await GetEntityByIdAsync(id);

        if (notification.UserId != CurrentUser.GetId())
        {
            await CheckPolicyAsync(ProcessManagementPermissions.Process.Manage);
        }

        notification.SetRead(Clock.Now);

        await _repository.UpdateAsync(notification, true);
    }

    public virtual async Task DismissAsync(DismissNotificationDto input)
    {
        var now = Clock.Now;
        var userId = CurrentUser.GetId();

        List<Notification> notifications = [];
        if (input.MaxCreationTime.HasValue)
        {
            notifications =
                await Repository.GetListAsync(x => x.CreationTime <= input.MaxCreationTime && x.UserId == userId);
        }

        if (input.NotificationIds != null)
        {
            foreach (var notificationId in input.NotificationIds.Where(notificationId =>
                         notifications.All(x => x.Id != notificationId)))
            {
                var notification = await Repository.FindAsync(notificationId);
                if (notification is null)
                {
                    continue;
                }

                if (notification.UserId != userId)
                {
                    await CheckPolicyAsync(ProcessManagementPermissions.Process.Manage);
                }

                notifications.Add(notification);
            }
        }

        foreach (var notification in notifications)
        {
            notification.SetDismissed(now);

            await _repository.UpdateAsync(notification);
        }

        if (UnitOfWorkManager.Current != null)
        {
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }
    }
}