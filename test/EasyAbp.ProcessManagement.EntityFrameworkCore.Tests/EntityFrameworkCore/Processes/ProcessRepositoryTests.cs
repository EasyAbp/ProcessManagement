using EasyAbp.ProcessManagement.Processes;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.Processes;

public class ProcessRepositoryTests : ProcessManagementEntityFrameworkCoreTestBase
{
    private readonly IProcessRepository _processRepository;

    public ProcessRepositoryTests()
    {
        _processRepository = GetRequiredService<IProcessRepository>();
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
