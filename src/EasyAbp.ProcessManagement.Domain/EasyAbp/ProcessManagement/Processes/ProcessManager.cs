using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Options;
using JetBrains.Annotations;
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

    public virtual Task<Process> CreateAsync(string processName, [CanBeNull] string customTag = null)
    {
        var processDefinition = Options.GetProcessDefinition(processName);

        return Task.FromResult(
            new Process(GuidGenerator.Create(), CurrentTenant.Id, processDefinition, Clock.Now, customTag));
    }

    public virtual Task UpdateStateAsync(Process process, IProcessState nextState, bool completeProcess)
    {
        var processDefinition = Options.GetProcessDefinition(process.ProcessName);

        var nextStates = processDefinition.GetNextStateNames(process.StateName);

        if (!nextStates.Contains(nextState.StateName))
        {
            throw new AbpException(
                $"The specified state `{nextState}` is invalid for the process `{process.ProcessName}`");
        }

        process.SetState(processDefinition, nextState);

        if (completeProcess)
        {
            process.CompleteProcess(Clock.Now);
        }

        return Task.CompletedTask;
    }
}