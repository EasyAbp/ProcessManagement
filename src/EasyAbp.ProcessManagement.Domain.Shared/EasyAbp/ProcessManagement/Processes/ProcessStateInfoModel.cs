using System;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class ProcessStateInfoModel : IProcessState
{
    /// <inheritdoc/>
    public virtual DateTime StateUpdateTime { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string? ActionName { get; protected set; }

    /// <inheritdoc/>
    public virtual ProcessStateFlag StateFlag { get; protected set; }

    /// <inheritdoc/>
    public virtual string? StateSummaryText { get; protected set; }

    public ProcessStateInfoModel()
    {
    }

    public ProcessStateInfoModel(DateTime stateUpdateTime, string stateName, IProcessStateCustom? stateCustom = null)
    {
        StateUpdateTime = stateUpdateTime;
        StateName = Check.NotNullOrWhiteSpace(stateName, nameof(stateName));
        ActionName = stateCustom?.ActionName;
        StateFlag = stateCustom?.StateFlag ?? default;
        StateSummaryText = stateCustom?.StateSummaryText;
    }
}