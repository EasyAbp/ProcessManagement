using System;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Processes;

public class InvalidStateUpdateTimeException : AbpException
{
    public InvalidStateUpdateTimeException(string fromStateName, string toStateName, string processName, Guid processId)
        : base(
            $"Failed to update from state `{fromStateName}` to state `{toStateName}` for the process " +
            $"`{processName}`(id: {processId}) since the StateUpdateTime is less than the current")
    {
    }
}