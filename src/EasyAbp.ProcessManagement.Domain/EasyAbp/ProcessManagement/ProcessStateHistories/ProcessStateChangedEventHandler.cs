using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Guids;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public class ProcessStateChangedEventHandler : ILocalEventHandler<ProcessStateChangedEto>, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;

    public ProcessStateChangedEventHandler(IGuidGenerator guidGenerator)
    {
        _guidGenerator = guidGenerator;
    }

    public virtual async Task HandleEventAsync(ProcessStateChangedEto eventData)
    {
        var history = new ProcessStateHistory(_guidGenerator.Create(), eventData.Process);
        
        // todo: save history entity.
    }
}