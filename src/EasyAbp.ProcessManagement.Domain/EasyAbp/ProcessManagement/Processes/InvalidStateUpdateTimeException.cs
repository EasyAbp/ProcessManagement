using System;
using Volo.Abp;

namespace EasyAbp.ProcessManagement.Processes;

public class InvalidStateUpdateTimeException : AbpException
{
    public InvalidStateUpdateTimeException(string stateName, string processName, Guid processId) : base(
        $"Failed to update to the state `{stateName}` for the process " +
        $"`{processName}`(id: {processId}) since the StateUpdateTime is less than the current")
    {
    }
}