using System;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Permissions;
using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.ProcessStateHistories.Dtos;
using EasyAbp.ProcessManagement.Security;
using Shouldly;
using Volo.Abp.Authorization;
using Volo.Abp.Users;
using Xunit;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public class ProcessStateHistoryAppServiceTests : ProcessManagementApplicationTestBase
{
    private readonly IProcessStateHistoryAppService _processStateHistoryAppService;
    private readonly IProcessRepository _processRepository;
    private readonly ProcessManager _processManager;
    private readonly ICurrentUser _currentUser;
    private readonly FakeAuthorizationService _authorizationService;

    public ProcessStateHistoryAppServiceTests()
    {
        _processStateHistoryAppService = GetRequiredService<IProcessStateHistoryAppService>();
        _processRepository = GetRequiredService<IProcessRepository>();
        _processManager = GetRequiredService<ProcessManager>();
        _currentUser = GetRequiredService<ICurrentUser>();
        _authorizationService = GetRequiredService<FakeAuthorizationService>();
    }

    [Fact]
    public async Task GetListAsync_Without_Manage_Should_Only_Allow_Own_Group_Processes()
    {
        // Arrange
        var ownProcess = await CreateProcessAsync($"U:{_currentUser.GetId()}");
        var otherProcess = await CreateProcessAsync($"U:{Guid.NewGuid()}");
        _authorizationService.DeniedPolicies.Add(ProcessManagementPermissions.Process.Manage);

        // Act
        var result = await _processStateHistoryAppService.GetListAsync(
            new ProcessStateHistoryGetListInput { ProcessId = ownProcess.Id });

        // Assert
        result.TotalCount.ShouldBe(1);
        result.Items.ShouldAllBe(x => x.ProcessId == ownProcess.Id);

        await Should.ThrowAsync<AbpAuthorizationException>(() => _processStateHistoryAppService.GetListAsync(
            new ProcessStateHistoryGetListInput { ProcessId = otherProcess.Id }));
    }

    [Fact]
    public async Task GetListAsync_With_Manage_Should_Allow_Another_Groups_Process()
    {
        // Arrange
        var otherProcess = await CreateProcessAsync($"U:{Guid.NewGuid()}");

        // Act
        var result = await _processStateHistoryAppService.GetListAsync(
            new ProcessStateHistoryGetListInput { ProcessId = otherProcess.Id });

        // Assert
        result.TotalCount.ShouldBe(1);
        result.Items.ShouldAllBe(x => x.ProcessId == otherProcess.Id);
    }

    private async Task<Process> CreateProcessAsync(string groupKey)
    {
        return await WithUnitOfWorkAsync(async () =>
        {
            var process = await _processManager.CreateAsync(
                new CreateProcessModel("FakeExport", null, groupKey), DateTime.Now);

            return await _processRepository.InsertAsync(process, autoSave: true);
        });
    }
}
