namespace EasyAbp.ProcessManagement.Processes;

public class ProcessAppServiceTests : ProcessManagementApplicationTestBase
{
    private readonly IProcessAppService _processAppService;

    public ProcessAppServiceTests()
    {
        _processAppService = GetRequiredService<IProcessAppService>();
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

