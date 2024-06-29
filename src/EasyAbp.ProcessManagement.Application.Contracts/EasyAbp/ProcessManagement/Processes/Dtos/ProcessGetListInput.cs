using System;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.ProcessManagement.Processes.Dtos;

[Serializable]
public class ProcessGetListInput : PagedAndSortedResultRequestDto
{
    public string? ProcessName { get; set; }

    public string? CorrelationId { get; set; }

    public string? GroupKey { get; set; }

    public DateTime? StateUpdateTime { get; set; }

    public string? StateName { get; set; }

    public string? ActionName { get; set; }

    public ProcessStateFlag? StateFlag { get; set; }

    public string? StateSummaryText { get; set; }

    public string? StateDetailsText { get; set; }
}