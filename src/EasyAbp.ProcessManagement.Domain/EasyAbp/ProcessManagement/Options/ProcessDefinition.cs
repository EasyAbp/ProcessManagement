using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Localization;

namespace EasyAbp.ProcessManagement.Options;

public class ProcessDefinition
{
    public string Name { get; }

    public ILocalizableString? DisplayName { get; }

    public string InitialStateName { get; protected set; } = null!;

    protected Dictionary<string, ProcessStateDefinition> StateDefinitions { get; } = new();

    public ProcessDefinition(string name, ILocalizableString? displayName)
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

        return StateDefinitions.TryGetValue(stateName, out var stateDefinition)
            ? stateDefinition
            : throw new UndefinedProcessStateException(stateName, Name);
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

    /// <summary>
    /// If the specified state is a child, grandchild, or further descendant of the current state, it returns true.
    /// </summary>
    public bool IsDescendantState(string stateName, string currentStateName)
    {
        var currentStateDefinition = StateDefinitions[currentStateName];

        if (currentStateDefinition.ChildrenStateNames.Contains(stateName))
        {
            return true;
        }

        return currentStateDefinition.ChildrenStateNames
            .SelectMany(x => StateDefinitions[x].ChildrenStateNames)
            .Contains(stateName);
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