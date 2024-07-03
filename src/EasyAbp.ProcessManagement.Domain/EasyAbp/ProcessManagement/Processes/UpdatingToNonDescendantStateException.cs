using System;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Processes;

public class UpdatingToNonDescendantStateException : AbpException
{
    public UpdatingToNonDescendantStateException(string stateName, string processName, Guid processId) : base(
        $"Skipping the event handling because the specified state `{stateName}` is not a descendant state of " +
        $"the current state for the process `{processName}` (id: {processId})")
    {
    }
}