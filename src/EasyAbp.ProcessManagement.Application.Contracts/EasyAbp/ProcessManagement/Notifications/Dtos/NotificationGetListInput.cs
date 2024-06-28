using System;
using System.ComponentModel;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.ProcessManagement.Notifications.Dtos;

[Serializable]
public class NotificationGetListInput : PagedAndSortedResultRequestDto
{
    public DateTime? FromCreationTime { get; set; }

    public DateTime? ToCreationTime { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ProcessId { get; set; }

    public DateTime? ReadTime { get; set; }

    public DateTime? DismissedTime { get; set; }

    public bool? Read { get; set; }

    public bool? Dismissed { get; set; }

    public string? ProcessName { get; set; }

    public string? CorrelationId { get; set; }

    public string? GroupKey { get; set; }

    public DateTime? CompletionTime { get; set; }

    public DateTime? StateUpdateTime { get; set; }

    public string? StateName { get; set; }

    public string? ActionName { get; set; }

    public ProcessStateFlag? StateFlag { get; set; }

    public string? StateSummaryText { get; set; }
}