using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using Volo.Abp.Application.Services;

namespace EasyAbp.ProcessManagement.Notifications;

public class NotificationAppService : ReadOnlyAppService<Notification, NotificationDto, Guid, NotificationGetListInput>,
    INotificationAppService
{
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
            .WhereIf(input.Read != null, x => x.Read == input.Read)
            .WhereIf(input.Dismissed != null, x => x.Dismissed == input.Dismissed)
            .WhereIf(!input.ProcessName.IsNullOrWhiteSpace(), x => x.ProcessName.Contains(input.ProcessName))
            .WhereIf(!input.CorrelationId.IsNullOrWhiteSpace(), x => x.CorrelationId.Contains(input.CorrelationId))
            .WhereIf(!input.GroupKey.IsNullOrWhiteSpace(), x => x.GroupKey.Contains(input.GroupKey))
            .WhereIf(input.CompletionTime != null, x => x.CompletionTime == input.CompletionTime)
            .WhereIf(input.StateUpdateTime != null, x => x.StateUpdateTime == input.StateUpdateTime)
            .WhereIf(!input.StateName.IsNullOrWhiteSpace(), x => x.StateName.Contains(input.StateName))
            .WhereIf(input.ActionName != null, x => x.ActionName == input.ActionName)
            .WhereIf(input.StateFlag != null, x => x.StateFlag == input.StateFlag)
            .WhereIf(input.StateSummaryText != null, x => x.StateSummaryText == input.StateSummaryText)
            ;
    }
}