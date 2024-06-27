using System;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.ProcessManagement.Notifications.Dtos;

[Serializable]
public class NotificationDto : CreationAuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }

    public Guid ProcessId { get; set; }

    public bool Read { get; set; }

    public bool Dismissed { get; set; }

    public string ProcessName { get; set; }

    public string CorrelationId { get; set; }

    public string GroupKey { get; set; }

    public DateTime? CompletionTime { get; set; }

    public DateTime StateUpdateTime { get; set; }

    public string StateName { get; set; }

    public string? ActionName { get; set; }

    public ProcessStateFlag StateFlag { get; set; }

    public string? StateSummaryText { get; set; }
}