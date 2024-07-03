using System;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Processes;

public class UpdatingToFutureStateException : AbpException
{
    public UpdatingToFutureStateException(string stateName, string processName, Guid processId) : base(
        $"Failed to update to the state `{stateName}` for the process `{processName}` (id: {processId}) since " +
        $"it's not a child. However, it's a descendant state, this error may be caused by the event disorder, " +
        $"so the next time the event handling is attempted, it may succeed.")
    {
    }
}