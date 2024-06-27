using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications;
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
}