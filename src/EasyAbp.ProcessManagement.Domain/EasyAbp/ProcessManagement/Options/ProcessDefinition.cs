using System.Collections.Generic;

namespace EasyAbp.ProcessManagement.Options;

public class ProcessDefinition
{
    public string Name { get; }

    public string DisplayName { get; }

    public string InitialStateName { get; protected set; } = null!;

    protected Dictionary<string, ProcessStateDefinition> StateDefinitions { get; } = new();

    public ProcessDefinition(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }

    public IEnumerable<string> GetNextStateNames(string currentStateName)
    {
        return StateDefinitions[currentStateName].NextStateNames;
    }

    public ProcessStateDefinition GetState(string stateName)
    {
        return StateDefinitions[stateName];
    }

    public ProcessDefinition AddState(ProcessStateDefinition stateDefinition)
    {
        StateDefinitions.Add(stateDefinition.Name, stateDefinition);
        return this;
    }

    public void SetNextState(string currentStateName, string nextStateName)
    {
        var stateDefinition = StateDefinitions[nextStateName];
        StateDefinitions[currentStateName].NextStateNames.Add(stateDefinition.Name);
    }

    public void SetNextStates(string currentStateName, IEnumerable<string> nextStateNames)
    {
        foreach (var nextStateName in nextStateNames)
        {
            SetNextState(currentStateName, nextStateName);
        }
    }

    public void SetInitialState(string stateName)
    {
        InitialStateName = stateName;
    }
}