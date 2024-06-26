using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace EasyAbp.ProcessManagement.Notifications;

public class ProcessChangedEventHandler : ITransientDependency,
    ILocalEventHandler<EntityCreatedEventData<Process>>,
    ILocalEventHandler<EntityUpdatedEventData<Process>>
{
    public virtual async Task HandleEventAsync(EntityCreatedEventData<Process> eventData)
    {
        // todo: create notifications.
    }

    public virtual async Task HandleEventAsync(EntityUpdatedEventData<Process> eventData)
    {
        // todo: create notifications.
    }
}