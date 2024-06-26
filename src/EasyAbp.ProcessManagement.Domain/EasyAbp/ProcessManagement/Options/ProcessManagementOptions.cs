﻿using System.Collections.Generic;

namespace EasyAbp.ProcessManagement.Options;

public class ProcessManagementOptions
{
    protected Dictionary<string, ProcessDefinition> ProcessDefinitions { get; } = new();

    public ProcessDefinition GetProcessDefinition(string processName)
    {
        return ProcessDefinitions[processName];
    }

    public void AddOrUpdateProcessDefinition(ProcessDefinition processDefinition)
    {
        ProcessDefinitions[processDefinition.Name] = processDefinition;
    }
}