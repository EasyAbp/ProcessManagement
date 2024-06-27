using System;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.Notifications;

public class NotificationRepositoryTests : ProcessManagementEntityFrameworkCoreTestBase
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationRepositoryTests()
    {
        _notificationRepository = GetRequiredService<INotificationRepository>();
    }

    /*
    [Fact]
    public async Task Test1()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            // Arrange

            // Act

            //Assert
        });
    }
    */
}
