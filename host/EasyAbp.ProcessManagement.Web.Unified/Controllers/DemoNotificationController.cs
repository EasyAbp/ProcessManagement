using System;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.UserGroups;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace EasyAbp.ProcessManagement.Controllers;

[Route("api/demo/notification")]
public class DemoNotificationController : AbpController
{
    private readonly IClock _clock;
    private readonly IdentityUserManager _identityUserManager;
    private readonly ProcessManager _processManager;
    private readonly IProcessRepository _processRepository;
    private readonly UserIdUserGroupContributor _userIdUserGroupContributor;

    private static readonly string[] StateNames =
        ["Ready", "Exporting", "Succeeded", "ExportFailed", "FailedToStartExporting"];

    private static readonly ProcessStateFlag[] StateFlags =
        [ProcessStateFlag.Information, ProcessStateFlag.Running, ProcessStateFlag.Success, ProcessStateFlag.Failure, ProcessStateFlag.Failure];

    private static readonly string[] SummaryTexts =
    [
        "Preparing to export...",
        "Loading the data...",
        "Congratulations! Export successful.",
        "Oops, the task failed!",
        "Could not start the exporting task."
    ];

    public DemoNotificationController(
        IClock clock,
        IdentityUserManager identityUserManager,
        ProcessManager processManager,
        IProcessRepository processRepository,
        UserIdUserGroupContributor userIdUserGroupContributor)
    {
        _clock = clock;
        _identityUserManager = identityUserManager;
        _processManager = processManager;
        _processRepository = processRepository;
        _userIdUserGroupContributor = userIdUserGroupContributor;
    }

    [HttpPost]
    [Route("random")]
    [UnitOfWork]
    public virtual async Task<IActionResult> CreateRandomAsync()
    {
        var adminUser = await _identityUserManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            return NotFound("Admin user not found.");
        }

        var now = _clock.Now;
        var random = new Random();
        var index = random.Next(StateNames.Length);

        var groupKey = await _userIdUserGroupContributor.CreateGroupKeyAsync(adminUser.Id.ToString());

        var process = await _processManager.CreateAsync(
            new CreateProcessModel("FakeExport", null, groupKey), now);

        // Advance through the state machine based on the target state.
        // State machine: Ready -> FailedToStartExporting
        //                Ready -> Exporting -> Succeeded | ExportFailed
        var targetState = StateNames[index];

        if (targetState == "FailedToStartExporting")
        {
            // Branches directly from Ready
            await _processManager.UpdateStateAsync(process,
                new UpdateProcessStateModel(now.AddSeconds(1), targetState,
                    targetState, StateFlags[index], SummaryTexts[index]));
        }
        else if (targetState != "Ready")
        {
            // Must go through Exporting first
            await _processManager.UpdateStateAsync(process,
                new UpdateProcessStateModel(now.AddSeconds(1), "Exporting",
                    "Loading data", ProcessStateFlag.Running, "Loading the data..."));

            if (targetState != "Exporting")
            {
                await _processManager.UpdateStateAsync(process,
                    new UpdateProcessStateModel(now.AddSeconds(2), targetState,
                        targetState, StateFlags[index], SummaryTexts[index]));
            }
        }

        await _processRepository.InsertAsync(process, true);

        return Ok(new
        {
            ProcessId = process.Id,
            StateName = process.StateName,
            StateFlag = process.StateFlag.ToString(),
            Summary = process.StateSummaryText
        });
    }
}
