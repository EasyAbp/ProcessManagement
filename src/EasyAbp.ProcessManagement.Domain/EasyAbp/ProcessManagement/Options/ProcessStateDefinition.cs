﻿using System.Collections.Generic;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Options;

public class ProcessStateDefinition
{
    /// <summary>
    /// Unique state name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Localized display name.
    /// todo: use ILocalizableString.
    /// </summary>
    public string? DisplayName { get; }

    /// <summary>
    /// Name of the father state. Stages can only transition from their father state.
    /// If null, this state is the initial state. A process can have only one initial state.
    /// </summary>
    internal string? FatherStateName { get; }

    /// <summary>
    /// Names of the children states. Stages can only transition from their father state.
    /// </summary>
    internal HashSet<string> ChildrenStateNames { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">Localized display name.</param>
    /// <param name="displayName">Localized display name.</param>
    /// <param name="fatherStateName">Name of the father state. Stages can only transition from their father state.
    /// If null, this state is the initial state. A process can have only one initial state.</param>
    public ProcessStateDefinition(string name, string? displayName, string? fatherStateName)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        DisplayName = displayName;
        FatherStateName = fatherStateName;
    }
}