using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace EasyAbp.ProcessManagement.Processes;

public class UpdateProcessStateEventHandler : IDistributedEventHandler<UpdateProcessStateEto>, ITransientDependency
{
    private readonly ProcessManager _processManager;
    private readonly IProcessRepository _processRepository;

    public UpdateProcessStateEventHandler(ProcessManager processManager,
        IProcessRepository processRepository)
    {
        _processManager = processManager;
        _processRepository = processRepository;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(UpdateProcessStateEto eventData)
    {
        var process = await _processRepository.GetAsync(x => x.CorrelationId == eventData.CorrelationId);

        await _processManager.UpdateStateAsync(process, eventData);

        await _processRepository.UpdateAsync(process, true);
    }
}