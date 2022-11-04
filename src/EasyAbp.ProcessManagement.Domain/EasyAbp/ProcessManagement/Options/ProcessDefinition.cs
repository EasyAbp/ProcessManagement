using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Options;

public class ProcessDefinition
{
    [NotNull]
    public string Name { get; }

    [CanBeNull]
    public string DisplayName { get; }

    public string InitialStateName { get; protected set; } = null!;

    protected Dictionary<string, ProcessStateDefinition> StateDefinitions { get; } = new();

    public ProcessDefinition([NotNull] string name, [CanBeNull] string displayName)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        DisplayName = displayName;
    }

    public IEnumerable<string> GetNextStateNames([NotNull] string currentStateName)
    {
        Check.NotNullOrWhiteSpace(currentStateName, nameof(currentStateName));

        return StateDefinitions[currentStateName].NextStateNames;
    }

    public ProcessStateDefinition GetState([NotNull] string stateName)
    {
        Check.NotNullOrWhiteSpace(stateName, nameof(stateName));

        return StateDefinitions[stateName];
    }

    public ProcessDefinition AddState([NotNull] ProcessStateDefinition stateDefinition)
    {
        Check.NotNull(stateDefinition, nameof(stateDefinition));

        StateDefinitions.Add(stateDefinition.Name, stateDefinition);
        return this;
    }

    public void SetNextState([NotNull] string currentStateName, [NotNull] string nextStateName)
    {
        Check.NotNullOrWhiteSpace(currentStateName, nameof(currentStateName));
        Check.NotNullOrWhiteSpace(nextStateName, nameof(nextStateName));

        var stateDefinition = StateDefinitions[nextStateName];
        StateDefinitions[currentStateName].NextStateNames.Add(stateDefinition.Name);
    }

    public void SetNextStates([NotNull] string currentStateName, IEnumerable<string> nextStateNames)
    {
        Check.NotNullOrWhiteSpace(currentStateName, nameof(currentStateName));

        foreach (var nextStateName in nextStateNames)
        {
            SetNextState(currentStateName, nextStateName);
        }
    }

    public void SetInitialState([NotNull] string stateName)
    {
        Check.NotNullOrWhiteSpace(stateName, nameof(stateName));

        InitialStateName = stateName;
    }
}