using System;
using JetBrains.Annotations;

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
    public virtual string StateDetailsText { get; protected set; }

    public ProcessStateInfoModel()
    {
    }

    public ProcessStateInfoModel(DateTime stateUpdateTime, [NotNull] string stateName, [CanBeNull] string subStateName,
        [CanBeNull] string detailsText)
    {
        StateUpdateTime = stateUpdateTime;
        StateName = stateName;
        SubStateName = subStateName;
        StateDetailsText = detailsText;
    }
}