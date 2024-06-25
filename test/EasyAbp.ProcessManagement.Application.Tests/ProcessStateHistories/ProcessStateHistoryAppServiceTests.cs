namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public class ProcessStateHistoryAppServiceTests : ProcessManagementApplicationTestBase
{
    private readonly IProcessStateHistoryAppService _processStateHistoryAppService;

    public ProcessStateHistoryAppServiceTests()
    {
        _processStateHistoryAppService = GetRequiredService<IProcessStateHistoryAppService>();
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

