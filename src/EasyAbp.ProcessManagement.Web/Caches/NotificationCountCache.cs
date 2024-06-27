using System;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications;
using EasyAbp.ProcessManagement.Notifications.Dtos;
using EasyAbp.ProcessManagement.Web.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace EasyAbp.ProcessManagement.Web.Caches;

public class NotificationCountCache : ITransientDependency
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;

    protected IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

    protected IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

    protected IDistributedCache<string> DistributedCache =>
        LazyServiceProvider.LazyGetRequiredService<IDistributedCache<string>>();

    protected ProcessManagementWebOptions Options =>
        LazyServiceProvider.LazyGetRequiredService<IOptions<ProcessManagementWebOptions>>().Value;

    private readonly ICurrentUser _currentUser;

    public NotificationCountCache(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public virtual async Task<int> GetOrAddAsync()
    {
        if (!_currentUser.IsAuthenticated)
        {
            return 0;
        }

        var cachedValue = await DistributedCache.GetOrAddAsync(GetCacheKey(), async () =>
        {
            // todo: abp framework bug...
            using var uow = UnitOfWorkManager.Begin();

            var notificationAppService = uow.ServiceProvider.GetRequiredService<INotificationAppService>();

            var result = await notificationAppService.GetListAsync(new NotificationGetListInput
            {
                MaxResultCount = 1,
                FromCreationTime = Clock.Now.Add(-Options.NotificationLifetime),
                UserId = _currentUser.GetId(),
                Dismissed = false,
                Read = false
            });

            return (result.TotalCount > 99 ? 99 : result.TotalCount).ToString();
        }, () => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5),
        });

        return cachedValue is null ? 0 : Convert.ToInt32(cachedValue);
    }

    public virtual async Task RemoveAsync() => await DistributedCache.RemoveAsync(GetCacheKey());

    protected virtual string GetCacheKey() => $"PM_NotificationCount_{_currentUser.GetId()}";
}