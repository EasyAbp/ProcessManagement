using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Options;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace EasyAbp.ProcessManagement.Processes;

public class ProcessManager : DomainService
{
    protected ProcessManagementOptions Options { get; }

    public ProcessManager(IOptions<ProcessManagementOptions> options)
    {
        Options = options.Value;
    }

    public virtual Task<Process> CreateAsync(string processName, DateTime now, string groupKey,
        IProcessStateCustom? stateCustom = null, string? correlationId = null)
    {
        var processDefinition = Options.GetProcessDefinition(processName);

        return Task.FromResult(new Process(GuidGenerator.Create(), CurrentTenant.Id, processDefinition, now,
            groupKey, stateCustom, correlationId));
    }

    public virtual Task UpdateStateAsync(Process process, IProcessState nextState, bool completeProcess)
    {
        var processDefinition = Options.GetProcessDefinition(process.ProcessName);

        var nextStates = processDefinition.GetChildStateNames(process.StateName);

        if (!nextStates.Contains(nextState.StateName))
        {
            throw new AbpException(
                $"The specified state `{nextState.StateName}` is invalid for the process `{process.ProcessName}`");
        }

        process.SetState(processDefinition, nextState);

        if (completeProcess)
        {
            process.CompleteProcess(Clock.Now);
        }

        return Task.CompletedTask;
    }
}