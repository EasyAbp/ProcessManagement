using System;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.ProcessManagement.ProcessStateHistories.Dtos;

[Serializable]
public class ProcessStateHistoryGetListInput : PagedAndSortedResultRequestDto
{
    public Guid ProcessId { get; set; }

    public string? ProcessName { get; set; }

    public string? StateName { get; set; }

    public string? ActionName { get; set; }

    public ProcessStateFlag? StateFlag { get; set; }

    public string? StateSummaryText { get; set; }

    public string? StateDetailsText { get; set; }

    public DateTime? StateUpdateTime { get; set; }
}