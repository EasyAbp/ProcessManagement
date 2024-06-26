using System;
using Volo.Abp;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class UpdateProcessStateModel : ExtensibleObject, IProcessState
{
    public string? ActionName { get; set; }

    public ProcessStateFlag StateFlag { get; set; }

    public string? StateSummaryText { get; set; }

    public DateTime StateUpdateTime { get; set; }

    public string StateName { get; set; }

    public string? StateDetailsText { get; set; }

    public UpdateProcessStateModel()
    {
    }

    public UpdateProcessStateModel(string? actionName, ProcessStateFlag stateFlag, string? stateSummaryText,
        DateTime stateUpdateTime, string stateName, string? stateDetailsText)
    {
        ActionName = actionName;
        StateFlag = stateFlag;
        StateSummaryText = stateSummaryText;
        StateUpdateTime = stateUpdateTime;
        StateName = Check.NotNullOrWhiteSpace(stateName, nameof(stateName));
        StateDetailsText = stateDetailsText;
    }
}