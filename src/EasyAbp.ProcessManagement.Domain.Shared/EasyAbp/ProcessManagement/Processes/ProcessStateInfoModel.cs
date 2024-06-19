using System;
using JetBrains.Annotations;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class ProcessStateInfoModel : IProcessState
{
    /// <inheritdoc/>
    public virtual string StateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string SubStateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string DetailsText { get; protected set; }

    /// <inheritdoc/>
    public virtual DateTime StateUpdateTime { get; protected set; }

    public ProcessStateInfoModel()
    {
    }

    public ProcessStateInfoModel([NotNull] string stateName, [CanBeNull] string subStateName,
        [CanBeNull] string detailsText, DateTime stateUpdateTime)
    {
        StateName = stateName;
        SubStateName = subStateName;
        DetailsText = detailsText;
        StateUpdateTime = stateUpdateTime;
    }
}