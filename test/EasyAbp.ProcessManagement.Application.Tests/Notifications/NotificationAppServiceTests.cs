using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.ProcessManagement.Notifications;

public class NotificationAppServiceTests : ProcessManagementApplicationTestBase
{
    private readonly INotificationAppService _notificationAppService;

    public NotificationAppServiceTests()
    {
        _notificationAppService = GetRequiredService<INotificationAppService>();
    }

    /*
    [Fact]
    public async Task Test1()
    {
        // Arrange

        // Act

        // Assert
    }
    */
}

