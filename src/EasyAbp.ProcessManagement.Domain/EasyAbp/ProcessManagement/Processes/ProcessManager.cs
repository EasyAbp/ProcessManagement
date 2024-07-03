using System;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Options;
using EasyAbp.ProcessManagement.ProcessStateHistories;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace EasyAbp.ProcessManagement.Processes;

public class ProcessManager : DomainService
{
    protected ProcessManagementOptions Options =>
        LazyServiceProvider.LazyGetRequiredService<IOptions<ProcessManagementOptions>>().Value;

    protected IProcessStateHistoryRepository ProcessStateHistoryRepository => LazyServiceProvider
        .LazyGetRequiredService<IProcessStateHistoryRepository>();

    public virtual async Task<Process> CreateAsync(CreateProcessModel model, DateTime now)
    {
        var processDefinition = Options.GetProcessDefinition(model.ProcessName);
        var stateDefinition = processDefinition.GetState(processDefinition.InitialStateName);

        model.StateFlag = NormalizeStateFlag(stateDefinition, model.StateFlag);
        var id = GuidGenerator.Create();

        var process = new Process(id, CurrentTenant.Id, processDefinition, now, model.GroupKey,
            model.CorrelationId ?? id.ToString(), model);

        await RecordStateHistoryAsync(process.Id, process);

        return process;
    }

    [UnitOfWork]
    public virtual async Task UpdateStateAsync(Process process, UpdateProcessStateModel state)
    {
        var processDefinition = Options.GetProcessDefinition(process.ProcessName);
        var stateDefinition = processDefinition.GetState(state.StateName);

        state.StateFlag = NormalizeStateFlag(stateDefinition, state.StateFlag);

        if (state.StateName != process.StateName)
        {
            await UpdateToDifferentStateAsync(processDefinition, stateDefinition, process, state);
        }
        else
        {
            await UpdateStateCustomInfoAsync(process, state);
        }
    }

    private static ProcessStateFlag NormalizeStateFlag(ProcessStateDefinition stateDefinition, ProcessStateFlag flag)
    {
        var newFlag = flag != ProcessStateFlag.Unspecified ? flag : stateDefinition.DefaultStateFlag;

        return newFlag == ProcessStateFlag.Unspecified ? ProcessStateFlag.Information : newFlag;
    }

    [UnitOfWork]
    protected virtual async Task UpdateToDifferentStateAsync(ProcessDefinition processDefinition,
        ProcessStateDefinition stateDefinition, Process process, UpdateProcessStateModel state)
    {
        var availableStates = processDefinition.GetChildrenStateNames(process.StateName);

        if (availableStates.Contains(state.StateName))
        {
            if (state.StateUpdateTime <= process.StateUpdateTime)
            {
                throw new InvalidStateUpdateTimeException(state.StateName, process.ProcessName, process.Id);
            }

            process.SetState(state);

            await RecordStateHistoryAsync(process.Id, state);
        }
        else
        {
            /* If this incoming state is a descendant of the current state, it will be accepted in the future.
             * So we throw an exception and skip handling it this time.
             * The next time the event handling is attempted, it may succeed.
             */
            if (processDefinition.IsDescendantState(state.StateName, process.StateName))
            {
                throw new UpdatingToFutureStateException(state.StateName, process.ProcessName, process.Id);
            }

            /*
             * Or, the process has been updated to this incoming state before, we just record the state history.
             */
            if ((await ProcessStateHistoryRepository.GetHistoriesByStateNameAsync(
                    process.Id, state.StateName)).Count != 0)
            {
                await RecordStateHistoryAsync(process.Id, state);
                return;
            }

            /*
             * Otherwise, this incoming state will never succeed, we don't handle it.
             */
            throw new UpdatingToNonDescendantStateException(state.StateName, process.ProcessName, process.Id);
        }
    }

    protected virtual async Task UpdateStateCustomInfoAsync(Process process, UpdateProcessStateModel state)
    {
        /* If it receives a state update event out of order (event.StateUpdateTime < process.StateUpdateTime),
         * we will only add a new state history entity without updating the process entity properties.
         */
        if (state.StateUpdateTime > process.StateUpdateTime)
        {
            process.SetState(state);
        }

        await RecordStateHistoryAsync(process.Id, state);
    }

    [UnitOfWork]
    protected virtual async Task<ProcessStateHistory> RecordStateHistoryAsync(Guid processId, IProcessState state)
    {
        return await ProcessStateHistoryRepository.InsertAsync(
            new ProcessStateHistory(GuidGenerator.Create(), CurrentTenant.Id, processId, state), true);
    }
}