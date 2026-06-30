using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.Notifications;

public class NotificationRepository : EfCoreRepository<IProcessManagementDbContext, Notification, Guid>, INotificationRepository
{
    public NotificationRepository(IDbContextProvider<IProcessManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<Notification>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    public virtual async Task DismissAllAsync(
        Guid userId,
        DateTime maxCreationTime,
        DateTime dismissedTime,
        CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);
        var queryable = await GetQueryableAsync();

        await queryable
            .Where(x => x.UserId == userId &&
                        x.CreationTime <= maxCreationTime &&
                        x.DismissedTime == null)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.DismissedTime, dismissedTime),
                token);
    }

    public virtual async Task DismissByProcessIdAsync(
        Guid processId,
        DateTime dismissedTime,
        CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);
        var queryable = await GetQueryableAsync();

        await queryable
            .Where(x => x.ProcessId == processId &&
                        x.DismissedTime == null)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.DismissedTime, dismissedTime),
                token);
    }
}
