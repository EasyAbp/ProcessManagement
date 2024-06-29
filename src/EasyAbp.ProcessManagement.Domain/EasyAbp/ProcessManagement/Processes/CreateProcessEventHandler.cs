using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace EasyAbp.ProcessManagement.Processes;

public class CreateProcessEventHandler : IDistributedEventHandler<CreateProcessEto>, ITransientDependency
{
    private readonly IClock _clock;
    private readonly ProcessManager _processManager;
    private readonly IProcessRepository _processRepository;

    public CreateProcessEventHandler(
        IClock clock,
        ProcessManager processManager,
        IProcessRepository processRepository)
    {
        _clock = clock;
        _processManager = processManager;
        _processRepository = processRepository;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(CreateProcessEto eventData)
    {
        var process = await _processManager.CreateAsync(eventData, _clock.Now);

        await _processRepository.InsertAsync(process, true);
    }
}