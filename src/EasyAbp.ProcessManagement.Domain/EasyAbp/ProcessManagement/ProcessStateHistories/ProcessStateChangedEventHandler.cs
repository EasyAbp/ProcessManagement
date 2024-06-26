using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public class ProcessStateChangedEventHandler : ILocalEventHandler<ProcessStateChangedEto>, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IProcessStateHistoryRepository _processStateHistoryRepository;

    public ProcessStateChangedEventHandler(
        IGuidGenerator guidGenerator,
        IProcessStateHistoryRepository processStateHistoryRepository)
    {
        _guidGenerator = guidGenerator;
        _processStateHistoryRepository = processStateHistoryRepository;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(ProcessStateChangedEto eventData)
    {
        var history = new ProcessStateHistory(
            _guidGenerator.Create(), eventData.TenantId, eventData.ProcessId, eventData.NewState);

        await _processStateHistoryRepository.InsertAsync(history, true);
    }
}