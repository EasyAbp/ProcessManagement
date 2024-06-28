using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Notifications;
using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.ProcessStateHistories;
using EasyAbp.ProcessManagement.UserGroups;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace EasyAbp.ProcessManagement;

public class DemoDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IClock _clock;
    private readonly IdentityUserManager _identityUserManager;
    private readonly ProcessManager _processManager;
    private readonly IProcessRepository _processRepository;
    private readonly IProcessStateHistoryRepository _processStateHistoryRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly UserIdUserGroupContributor _userIdUserGroupContributor;

    public DemoDataSeedContributor(
        IClock clock,
        IdentityUserManager identityUserManager,
        ProcessManager processManager,
        IProcessRepository processRepository,
        IProcessStateHistoryRepository processStateHistoryRepository,
        INotificationRepository notificationRepository,
        UserIdUserGroupContributor userIdUserGroupContributor)
    {
        _clock = clock;
        _identityUserManager = identityUserManager;
        _processManager = processManager;
        _processRepository = processRepository;
        _processStateHistoryRepository = processStateHistoryRepository;
        _notificationRepository = notificationRepository;
        _userIdUserGroupContributor = userIdUserGroupContributor;
    }

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await SeedDemoProcessAsync(context);
    }

    [UnitOfWork]
    protected virtual async Task SeedDemoProcessAsync(DataSeedContext context)
    {
        var processes = await _processRepository.GetListAsync(x => x.ProcessName == "FakeExport");

        // delete all demo process entities.
        await _processRepository.DeleteManyAsync(processes, true);
        await _processStateHistoryRepository.DeleteAsync(x => processes.Select(y => y.Id).Contains(x.ProcessId));
        await _notificationRepository.DeleteAsync(x => processes.Select(y => y.Id).Contains(x.ProcessId));

        var adminUser = await _identityUserManager.FindByNameAsync("admin");
        var now = _clock.Now;

        var process1 = await _processManager.CreateAsync("FakeExport", now.AddHours(-5),
            await _userIdUserGroupContributor.CreateGroupKeyAsync(adminUser!.Id.ToString()));

        await _processManager.UpdateStateAsync(process1,
            new ProcessStateInfoModel(now.AddHours(-4), "Exporting",
                new ProcessStateCustomModel("Loading data",
                    ProcessStateFlag.Running,
                    "Loading the data...",
                    "<b>Loading the data...</b>")), false);

        await _processManager.UpdateStateAsync(process1,
            new ProcessStateInfoModel(now.AddHours(-3), "Exporting",
                new ProcessStateCustomModel("Exporting to ZIP",
                    ProcessStateFlag.Running,
                    "Exporting to the .zip file...",
                    "<b>Exporting to the .zip file...</b>")), false);

        await _processManager.UpdateStateAsync(process1,
            new ProcessStateInfoModel(now.AddHours(-2), "Succeeded",
                new ProcessStateCustomModel("Export is done!",
                    ProcessStateFlag.Success,
                    "Congratulations! Export successful.",
                    "<b>Congratulations!</b><br/>Everything is done.")), false);

        await _processRepository.InsertAsync(process1, true);

        var process2 = await _processManager.CreateAsync("FakeExport", now,
            await _userIdUserGroupContributor.CreateGroupKeyAsync(adminUser!.Id.ToString()));

        await _processManager.UpdateStateAsync(process2,
            new ProcessStateInfoModel(now.AddHours(-1), "Exporting",
                new ProcessStateCustomModel("Loading data",
                    ProcessStateFlag.Running,
                    "Loading the data...",
                    "<b>Loading the data...</b>")), false);

        await _processRepository.InsertAsync(process2, true);

        var process3 = await _processManager.CreateAsync("FakeExport", now,
            await _userIdUserGroupContributor.CreateGroupKeyAsync(adminUser!.Id.ToString()));

        await _processManager.UpdateStateAsync(process3,
            new ProcessStateInfoModel(now.AddHours(-2), "Exporting",
                new ProcessStateCustomModel("Loading data",
                    ProcessStateFlag.Running,
                    "Loading the data...",
                    "<b>Loading the data...</b>")), false);

        await _processManager.UpdateStateAsync(process3,
            new ProcessStateInfoModel(now.AddHours(-1), "Failed",
                new ProcessStateCustomModel("Failed...",
                    ProcessStateFlag.Failure,
                    "Oops, the task failed!",
                    null)), false);

        await _processRepository.InsertAsync(process3, true);
    }
}