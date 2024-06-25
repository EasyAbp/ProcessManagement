using EasyAbp.ProcessManagement.ProcessStateHistories;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.ProcessStateHistories;

public class ProcessStateHistoryRepositoryTests : ProcessManagementEntityFrameworkCoreTestBase
{
    private readonly IProcessStateHistoryRepository _processStateHistoryRepository;

    public ProcessStateHistoryRepositoryTests()
    {
        _processStateHistoryRepository = GetRequiredService<IProcessStateHistoryRepository>();
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
