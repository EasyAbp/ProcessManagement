using System;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.ProcessManagement.ProcessStateHistories.Dtos;

[Serializable]
public class ProcessStateHistoryDto : EntityDto<Guid>, IProcessState
{
    public Guid ProcessId { get; set; }

    public string ProcessName { get; set; }

    public string StateName { get; set; }

    public string? ActionName { get; set; }

    public ProcessStateFlag StateFlag { get; set; }

    public string? StateSummaryText { get; set; }

    public string? StateDetailsText { get; set; }

    public DateTime StateUpdateTime { get; set; }

    #region Out of the entity

    public string ProcessDisplayName { get; set; }

    public string StateDisplayName { get; set; }

    #endregion
}