using System;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Processes;

public class UpdatingToFutureStateException : AbpException
{
    public UpdatingToFutureStateException(string fromStateName, string toStateName, string processName, Guid processId)
        : base(
            $"Failed to update from state `{fromStateName}` to state `{toStateName}` for the process `{processName}`" +
            $" (id: {processId}) since it's not a child. However, it's a descendant state, this error may be caused" +
            $" by the event disorder, so the next time the event handling is attempted, it may succeed.")
    {
    }
}