using System;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class ProcessStateCustomModel : IProcessStateCustom
{
    public string? SubStateName { get; set; }

    public ProcessStateFlag StateFlag { get; set; }

    public string? StateSummaryText { get; set; }

    public string? StateDetailsText { get; set; }

    public ProcessStateCustomModel()
    {
    }

    public ProcessStateCustomModel(string? subStateName, ProcessStateFlag stateFlag, string? stateSummaryText,
        string? stateDetailsText)
    {
        SubStateName = subStateName;
        StateFlag = stateFlag;
        StateSummaryText = stateSummaryText;
        StateDetailsText = stateDetailsText;
    }
}