using System.Collections.Generic;
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

    public IEnumerable<string> GetChildStateNames(string currentStateName)
    {
        Check.NotNullOrWhiteSpace(currentStateName, nameof(currentStateName));

        return StateDefinitions[currentStateName].NextStateNames;
    }

    public ProcessStateDefinition GetState(string stateName)
    {
        Check.NotNullOrWhiteSpace(stateName, nameof(stateName));

        return StateDefinitions[stateName];
    }

    /// <summary>
    /// Add a state.
    /// </summary>
    /// <param name="stateDefinition">The state definition object.</param>
    /// <param name="parentStateNames">Names of the parent states. Stages can only transition from their parent states.</param>
    /// <returns></returns>
    public ProcessDefinition AddState(ProcessStateDefinition stateDefinition, params string[]? parentStateNames)
    {
        Check.NotNull(stateDefinition, nameof(stateDefinition));

        if (StateDefinitions.ContainsKey(stateDefinition.Name))
        {
            throw new AbpException(
                $"Duplicate state definition ({stateDefinition.Name}) for the process ({Name}) was found.");
        }

        StateDefinitions.Add(stateDefinition.Name, stateDefinition);

        if (parentStateNames is null)
        {
            SetInitialState(stateDefinition.Name);
        }
        else
        {
            foreach (var parentStateName in parentStateNames)
            {
                LinkStates(stateDefinition.Name, parentStateName);
            }
        }

        return this;
    }

    private void LinkStates(string stateName, string parentStateName)
    {
        Check.NotNullOrWhiteSpace(stateName, nameof(stateName));
        Check.NotNullOrWhiteSpace(parentStateName, nameof(parentStateName));

        var stateDefinition = StateDefinitions[stateName];

        StateDefinitions[parentStateName].NextStateNames.Add(stateDefinition.Name);
    }

    private void SetInitialState(string stateName)
    {
        Check.NotNullOrWhiteSpace(stateName, nameof(stateName));

        if (InitialStateName is not null)
        {
            throw new AbpException($"There should be only one initial state for the process ({Name}).");
        }

        InitialStateName = stateName;
    }
}