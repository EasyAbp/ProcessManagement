using System;
using EasyAbp.ProcessManagement.Options;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace EasyAbp.ProcessManagement.Processes;

public class ProcessStateHistory : Entity, IHasExtraProperties
{
    /// <summary>
    /// <see cref="Process"/> Id.
    /// </summary>
    public virtual Guid ProcessId { get; protected set; }

    /// <summary>
    /// The sequence of state changes.
    /// </summary>
    public virtual int Order { get; protected set; }

    /// <summary>
    /// Record the Name value of <see cref="ProcessStateDefinition"/>.
    /// </summary>
    public virtual string StateName { get; protected set; }

    /// <summary>
    /// The time when the process changed to this state.
    /// </summary>
    public virtual DateTime TimeToGetState { get; protected set; }

    /// <summary>
    /// ABP extra properties. It can be used to record such as `DepartmentManagerId` and `CustomerRemark`.
    /// </summary>
    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public override object[] GetKeys()
    {
        return new object[] { ProcessId, Order };
    }

    protected ProcessStateHistory()
    {
    }

    public ProcessStateHistory(Guid processId, int order, string stateName, DateTime timeToGetState)
    {
        ProcessId = processId;
        Order = order;
        StateName = stateName;
        TimeToGetState = timeToGetState;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }
}