using System;
using Volo.Abp;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class UpdateProcessStateModel : ExtensibleObject, IProcessState
{
    public DateTime StateUpdateTime { get; set; }

    public string StateName { get; set; }

    public string? ActionName { get; set; }

    public ProcessStateFlag StateFlag { get; set; }

    public string? StateSummaryText { get; set; }


    public string? StateDetailsText { get; set; }

    public UpdateProcessStateModel()
    {
    }

    public UpdateProcessStateModel(DateTime stateUpdateTime, string stateName)
    {
        StateUpdateTime = stateUpdateTime;
        StateName = Check.NotNullOrWhiteSpace(stateName, nameof(stateName));
    }

    public UpdateProcessStateModel(DateTime stateUpdateTime, string stateName, string? actionName,
        ProcessStateFlag stateFlag, string? stateSummaryText, string? stateDetailsText)
    {
        StateUpdateTime = stateUpdateTime;
        StateName = Check.NotNullOrWhiteSpace(stateName, nameof(stateName));
        ActionName = actionName;
        StateFlag = stateFlag;
        StateSummaryText = stateSummaryText;
        StateDetailsText = stateDetailsText;
    }
}