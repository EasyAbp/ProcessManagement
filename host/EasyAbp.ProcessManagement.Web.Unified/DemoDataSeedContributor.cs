using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Processes;
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
    private readonly UserIdUserGroupContributor _userIdUserGroupContributor;

    public DemoDataSeedContributor(
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

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await SeedDemoProcessAsync(context);
    }

    [UnitOfWork]
    protected virtual async Task SeedDemoProcessAsync(DataSeedContext context)
    {
        // delete all demo process entities.
        await _processRepository.DeleteAsync(x => x.ProcessName == "FakeExport");

        var adminUser = await _identityUserManager.FindByNameAsync("admin");

        var process = await _processManager.CreateAsync("FakeExport", _clock.Now,
            await _userIdUserGroupContributor.CreateGroupKeyAsync(adminUser!.Id.ToString()));

        await _processRepository.InsertAsync(process, true);
    }
}