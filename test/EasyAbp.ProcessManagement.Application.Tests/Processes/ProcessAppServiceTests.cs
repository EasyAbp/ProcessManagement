using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Permissions;
using EasyAbp.ProcessManagement.Processes.Dtos;
using EasyAbp.ProcessManagement.Security;
using Shouldly;
using Volo.Abp.Authorization;
using Volo.Abp.Users;
using Xunit;

namespace EasyAbp.ProcessManagement.Processes;

public class ProcessAppServiceTests : ProcessManagementApplicationTestBase
{
    private readonly IProcessAppService _processAppService;
    private readonly IProcessRepository _processRepository;
    private readonly ProcessManager _processManager;
    private readonly ICurrentUser _currentUser;
    private readonly FakeAuthorizationService _authorizationService;

    public ProcessAppServiceTests()
    {
        _processAppService = GetRequiredService<IProcessAppService>();
        _processRepository = GetRequiredService<IProcessRepository>();
        _processManager = GetRequiredService<ProcessManager>();
        _currentUser = GetRequiredService<ICurrentUser>();
        _authorizationService = GetRequiredService<FakeAuthorizationService>();
    }

    [Fact]
    public async Task GetListAsync_Without_Manage_And_Without_UserName_Should_Only_Return_Own_Group_Processes()
    {
        // Arrange
        var (ownProcess, otherProcess) = await CreateOwnAndOtherUsersProcessesAsync();
        _authorizationService.DeniedPolicies.Add(ProcessManagementPermissions.Process.Manage);

        // Act
        var result = await _processAppService.GetListAsync(new ProcessGetListInput());

        // Assert
        result.TotalCount.ShouldBe(1);
        result.Items.ShouldContain(x => x.Id == ownProcess.Id);
        result.Items.ShouldNotContain(x => x.Id == otherProcess.Id);
    }

    [Fact]
    public async Task GetListAsync_Without_Manage_And_With_Own_UserName_Should_Only_Return_Own_Group_Processes()
    {
        // Arrange
        var (ownProcess, otherProcess) = await CreateOwnAndOtherUsersProcessesAsync();
        _authorizationService.DeniedPolicies.Add(ProcessManagementPermissions.Process.Manage);

        // Act
        var result = await _processAppService.GetListAsync(new ProcessGetListInput
        {
            UserName = _currentUser.UserName
        });

        // Assert
        result.TotalCount.ShouldBe(1);
        result.Items.ShouldContain(x => x.Id == ownProcess.Id);
        result.Items.ShouldNotContain(x => x.Id == otherProcess.Id);
    }

    [Fact]
    public async Task GetListAsync_Without_Manage_And_With_Another_UserName_Should_Throw()
    {
        // Arrange
        await CreateOwnAndOtherUsersProcessesAsync();
        _authorizationService.DeniedPolicies.Add(ProcessManagementPermissions.Process.Manage);

        // Act & Assert
        await Should.ThrowAsync<AbpAuthorizationException>(() =>
            _processAppService.GetListAsync(new ProcessGetListInput { UserName = "someone-else" }));
    }

    [Fact]
    public async Task GetListAsync_With_Manage_And_Without_UserName_Should_Return_All_Processes()
    {
        // Arrange
        var (ownProcess, otherProcess) = await CreateOwnAndOtherUsersProcessesAsync();

        // Act
        var result = await _processAppService.GetListAsync(new ProcessGetListInput());

        // Assert
        result.TotalCount.ShouldBe(2);
        result.Items.ShouldContain(x => x.Id == ownProcess.Id);
        result.Items.ShouldContain(x => x.Id == otherProcess.Id);
    }

    [Fact]
    public async Task GetAsync_Without_Manage_Should_Throw_For_Another_Groups_Process()
    {
        // Arrange
        var (ownProcess, otherProcess) = await CreateOwnAndOtherUsersProcessesAsync();
        _authorizationService.DeniedPolicies.Add(ProcessManagementPermissions.Process.Manage);

        // Act & Assert
        (await _processAppService.GetAsync(ownProcess.Id)).Id.ShouldBe(ownProcess.Id);
        await Should.ThrowAsync<AbpAuthorizationException>(() => _processAppService.GetAsync(otherProcess.Id));
    }

    private async Task<(Process OwnProcess, Process OtherProcess)> CreateOwnAndOtherUsersProcessesAsync()
    {
        return await WithUnitOfWorkAsync(async () =>
        {
            var ownProcess = await _processManager.CreateAsync(
                new CreateProcessModel("FakeExport", null, $"U:{_currentUser.GetId()}"), DateTime.Now);
            var otherProcess = await _processManager.CreateAsync(
                new CreateProcessModel("FakeExport", null, $"U:{Guid.NewGuid()}"), DateTime.Now);

            await _processRepository.InsertManyAsync([ownProcess, otherProcess], autoSave: true);

            return (ownProcess, otherProcess);
        });
    }
}
