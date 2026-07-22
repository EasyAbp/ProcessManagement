using EasyAbp.ProcessManagement.Notifications;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.Processes.Dtos;
using EasyAbp.ProcessManagement.ProcessStateHistories;
using EasyAbp.ProcessManagement.ProcessStateHistories.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace EasyAbp.ProcessManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProcessToProcessDtoMapper : MapperBase<Process, ProcessDto>
{
    [MapperIgnoreTarget(nameof(ProcessDto.ProcessDisplayName))]
    [MapperIgnoreTarget(nameof(ProcessDto.StateDisplayName))]
    public override partial ProcessDto Map(Process source);

    [MapperIgnoreTarget(nameof(ProcessDto.ProcessDisplayName))]
    [MapperIgnoreTarget(nameof(ProcessDto.StateDisplayName))]
    public override partial void Map(Process source, ProcessDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProcessStateHistoryToProcessStateHistoryDtoMapper : MapperBase<ProcessStateHistory, ProcessStateHistoryDto>
{
    [MapperIgnoreTarget(nameof(ProcessStateHistoryDto.ProcessDisplayName))]
    [MapperIgnoreTarget(nameof(ProcessStateHistoryDto.StateDisplayName))]
    public override partial ProcessStateHistoryDto Map(ProcessStateHistory source);

    [MapperIgnoreTarget(nameof(ProcessStateHistoryDto.ProcessDisplayName))]
    [MapperIgnoreTarget(nameof(ProcessStateHistoryDto.StateDisplayName))]
    public override partial void Map(ProcessStateHistory source, ProcessStateHistoryDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class NotificationToNotificationDtoMapper : MapperBase<Notification, NotificationDto>
{
    [MapperIgnoreTarget(nameof(NotificationDto.ProcessDisplayName))]
    [MapperIgnoreTarget(nameof(NotificationDto.StateDisplayName))]
    public override partial NotificationDto Map(Notification source);

    [MapperIgnoreTarget(nameof(NotificationDto.ProcessDisplayName))]
    [MapperIgnoreTarget(nameof(NotificationDto.StateDisplayName))]
    public override partial void Map(Notification source, NotificationDto destination);
}
