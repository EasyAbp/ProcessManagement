using System;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class CreateProcessModel : ExtensibleObject, IProcessBase, IProcessStateCustom
{
    public string ProcessName { get; set; }

    /// <summary>
    /// A unique correlation ID. If not set, this value will be auto-set to the value of the Id property.
    /// </summary>
    public string? CorrelationId { get; set; }

    public string GroupKey { get; set; }

    public string? ActionName { get; set; }

    public ProcessStateFlag StateFlag { get; set; }

    public string? StateSummaryText { get; set; }

    public CreateProcessModel()
    {
    }

    public CreateProcessModel(string processName, string? correlationId, string groupKey, string? actionName,
        ProcessStateFlag stateFlag, string? stateSummaryText)
    {
        ProcessName = Check.NotNullOrWhiteSpace(processName, nameof(processName));
        CorrelationId = correlationId;
        GroupKey = Check.NotNull(groupKey, nameof(groupKey));
        ActionName = actionName;
        StateFlag = stateFlag;
        StateSummaryText = stateSummaryText;
    }

    public CreateProcessModel(string processName, string? correlationId, string groupKey)
    {
        ProcessName = Check.NotNullOrWhiteSpace(processName, nameof(processName));
        CorrelationId = correlationId;
        GroupKey = Check.NotNull(groupKey, nameof(groupKey));
    }
}