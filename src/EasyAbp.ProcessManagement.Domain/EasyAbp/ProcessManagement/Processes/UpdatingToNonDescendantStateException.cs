using System;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Processes;

public class UpdatingToNonDescendantStateException : AbpException
{
    public UpdatingToNonDescendantStateException(
        string fromStateName, string toStateName, string processName, Guid processId) : base(
        $"Skipping the event handling because the specified state `{toStateName}` is not a descendant state of " +
        $"the current state ({fromStateName}) for the process `{processName}` (id: {processId})")
    {
    }
}