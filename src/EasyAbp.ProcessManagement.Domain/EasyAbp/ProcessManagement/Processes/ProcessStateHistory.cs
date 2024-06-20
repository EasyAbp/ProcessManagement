using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.ProcessManagement.Processes;

public class ProcessStateHistory : Entity, IProcessState, IHasExtraProperties
{
    public virtual Guid ProcessId { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string SubStateName { get; protected set; }

    /// <inheritdoc/>
    public virtual string StateDetailsText { get; protected set; }

    /// <inheritdoc/>
    public virtual DateTime StateUpdateTime { get; protected set; }

    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public override object[] GetKeys()
    {
        return [ProcessId, StateUpdateTime];
    }

    protected ProcessStateHistory()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public ProcessStateHistory(Guid processId, IProcessState processState)
    {
        ProcessId = processId;

        StateName = processState.StateName;
        SubStateName = processState.SubStateName;
        StateDetailsText = processState.StateDetailsText;
        StateUpdateTime = processState.StateUpdateTime;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }
}