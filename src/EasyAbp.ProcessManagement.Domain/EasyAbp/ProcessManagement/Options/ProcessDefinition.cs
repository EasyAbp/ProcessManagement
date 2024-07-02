using System.Collections.Generic;
using System.Linq;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Options;

public class ProcessDefinition
{
    public string Name { get; }

    public string? DisplayName { get; }

    public string InitialStateName { get; protected set; } = null!;

    protected Dictionary<string, ProcessStateDefinition> StateDefinitions { get; } = new();

    public ProcessDefinition(string name, string? displayName)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        DisplayName = displayName;
    }

    public List<string> GetChildrenStateNames(string currentStateName)
    {
        Check.NotNullOrWhiteSpace(currentStateName, nameof(currentStateName));

        return StateDefinitions[currentStateName].ChildrenStateNames.ToList();
    }

    public ProcessStateDefinition GetState(string stateName)
    {
        Check.NotNullOrWhiteSpace(stateName, nameof(stateName));

        return StateDefinitions[stateName];
    }

    public ProcessDefinition AddState(ProcessStateDefinition stateDefinition)
    {
        Check.NotNull(stateDefinition, nameof(stateDefinition));

        if (StateDefinitions.ContainsKey(stateDefinition.Name))
        {
            throw new AbpException(
                $"Duplicate state definition ({stateDefinition.Name}) for the process ({Name}) was found.");
        }

        StateDefinitions.Add(stateDefinition.Name, stateDefinition);

        if (stateDefinition.FatherStateName is null)
        {
            SetAsInitialState(stateDefinition.Name);
        }
        else
        {
            SetAsChildState(stateDefinition.Name, stateDefinition.FatherStateName);
        }

        return this;
    }

    private void SetAsChildState(string stateName, string fatherStateName)
    {
        Check.NotNullOrWhiteSpace(stateName, nameof(stateName));
        Check.NotNullOrWhiteSpace(fatherStateName, nameof(fatherStateName));

        var stateDefinition = StateDefinitions[stateName];

        StateDefinitions[fatherStateName].ChildrenStateNames.Add(stateDefinition.Name);
    }

    private void SetAsInitialState(string stateName)
    {
        Check.NotNullOrWhiteSpace(stateName, nameof(stateName));

        if (InitialStateName is not null)
        {
            throw new AbpException($"There should be only one initial state for the process ({Name}).");
        }

        InitialStateName = stateName;
    }
}