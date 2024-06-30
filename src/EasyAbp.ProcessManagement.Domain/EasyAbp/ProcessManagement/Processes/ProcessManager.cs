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

    public virtual Task<Process> CreateAsync(CreateProcessModel model, DateTime now)
    {
        var processDefinition = Options.GetProcessDefinition(model.ProcessName);

        var id = GuidGenerator.Create();

        return Task.FromResult(new Process(id, CurrentTenant.Id, processDefinition, now, model.GroupKey,
            model.CorrelationId ?? id.ToString(), model));
    }

    public virtual Task UpdateStateAsync(Process process, IProcessState nextState)
    {
        if (nextState.StateName != process.StateName)
        {
            var processDefinition = Options.GetProcessDefinition(process.ProcessName);

            var nextStates = processDefinition.GetChildStateNames(process.StateName);

            if (!nextStates.Contains(nextState.StateName))
            {
                throw new AbpException(
                    $"The specified state `{nextState.StateName}` is invalid for the process `{process.ProcessName}`");
            }
        }

        process.SetState(nextState);

        return Task.CompletedTask;
    }
}