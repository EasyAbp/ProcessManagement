using System;
using JetBrains.Annotations;
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
    public virtual string SubStateName { get; protected set; }

    /// <inheritdoc/>
    public virtual ProcessStateFlag StateFlag { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateSummaryText { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateDetailsText { get; protected set; }

    public ProcessStateInfoModel()
    {
    }

    public ProcessStateInfoModel(DateTime stateUpdateTime, [NotNull] string stateName, [CanBeNull] string subStateName,
        ProcessStateFlag stateFlag, [CanBeNull] string stateSummaryText, [CanBeNull] string stateDetailsText)
    {
        StateUpdateTime = stateUpdateTime;
        StateName = Check.NotNullOrWhiteSpace(stateName, nameof(stateName));
        SubStateName = subStateName;
        StateFlag = stateFlag;
        StateSummaryText = stateSummaryText;
        StateDetailsText = stateDetailsText;
    }
}