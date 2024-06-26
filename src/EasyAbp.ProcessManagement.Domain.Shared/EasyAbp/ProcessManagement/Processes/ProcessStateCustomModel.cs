using System;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class ProcessStateCustomModel : IProcessStateCustom
{
    public string? ActionName { get; set; }

    public ProcessStateFlag StateFlag { get; set; }

    public string? StateSummaryText { get; set; }

    public string? StateDetailsText { get; set; }

    public ProcessStateCustomModel()
    {
    }

    public ProcessStateCustomModel(string? actionName, ProcessStateFlag stateFlag, string? stateSummaryText,
        string? stateDetailsText)
    {
        ActionName = actionName;
        StateFlag = stateFlag;
        StateSummaryText = stateSummaryText;
        StateDetailsText = stateDetailsText;
    }
}