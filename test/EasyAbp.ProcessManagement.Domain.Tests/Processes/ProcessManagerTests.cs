using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Options;
using EasyAbp.ProcessManagement.ProcessStateHistories;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace EasyAbp.ProcessManagement.Processes;

public class ProcessManagerTests : ProcessManagementDomainTestBase
{
    protected ProcessManager ProcessManager { get; }
    protected IProcessRepository ProcessRepository { get; }
    protected IProcessStateHistoryRepository ProcessStateHistoryRepository { get; }
    protected ProcessManagementOptions Options { get; }

    public ProcessManagerTests()
    {
        ProcessManager = GetRequiredService<ProcessManager>();
        ProcessRepository = GetRequiredService<IProcessRepository>();
        ProcessStateHistoryRepository = GetRequiredService<IProcessStateHistoryRepository>();
        Options = GetRequiredService<IOptions<ProcessManagementOptions>>().Value;
    }

    [Fact]
    public async Task Should_Create_State()
    {
        var now = DateTime.Now;

        var process = await ProcessManager.CreateAsync(new CreateProcessModel("FakeExport", null, "groupKey"), now);

        await ProcessRepository.InsertAsync(process, true);

        process.ProcessName.ShouldBe("FakeExport");
        process.CorrelationId.ShouldBe(process.Id.ToString());
        process.GroupKey.ShouldBe("groupKey");
        process.StateUpdateTime.ShouldBe(now);

        var histories = await ProcessStateHistoryRepository.GetListAsync(x => x.ProcessId == process.Id);

        histories.Count.ShouldBe(1);
        histories.ShouldContain(x => x.StateName == "Ready" && x.StateUpdateTime == now);
    }

    [Fact]
    public async Task Should_Update_To_Child_State()
    {
        var process = await ProcessManager.CreateAsync(
            new CreateProcessModel("FakeExport", null, "groupKey"), DateTime.Now);

        await ProcessRepository.InsertAsync(process, true);

        var updateTime = DateTime.Now;

        await ProcessManager.UpdateStateAsync(process, new UpdateProcessStateModel(updateTime, "Exporting"));

        await ProcessRepository.UpdateAsync(process, true);

        process.StateName.ShouldBe("Exporting");
        process.StateUpdateTime.ShouldBe(updateTime);

        var histories = await ProcessStateHistoryRepository.GetListAsync(x => x.ProcessId == process.Id);

        histories.Count.ShouldBe(2);
        histories.ShouldContain(x => x.StateName == "Ready");
        histories.ShouldContain(x => x.StateName == "Exporting" && x.StateUpdateTime == updateTime);
    }

    [Fact]
    public async Task Should_Update_State_Custom_Info()
    {
        var process = await ProcessManager.CreateAsync(
            new CreateProcessModel("FakeExport", null, "groupKey", "hi", ProcessStateFlag.Information, null),
            DateTime.Now);

        process.StateFlag.ShouldBe(ProcessStateFlag.Information);

        await ProcessRepository.InsertAsync(process, true);

        var updateTime = DateTime.Now;

        // Not updating from Exporting to Ready, but add a history for Ready.
        await ProcessManager.UpdateStateAsync(process,
            new UpdateProcessStateModel(updateTime, "Ready", "balalala", ProcessStateFlag.Running, null));

        var histories = await ProcessStateHistoryRepository.GetListAsync(x => x.ProcessId == process.Id);

        histories.Count.ShouldBe(2);
        histories.Count(x => x.StateName == "Ready").ShouldBe(2);
        histories.ShouldContain(x =>
            x.StateName == "Ready" && x.ActionName == "balalala" && x.StateFlag == ProcessStateFlag.Running &&
            x.StateUpdateTime == updateTime);
    }

    [Fact]
    public async Task Should_Update_State_Custom_Info_Even_If_Disordered()
    {
        var process = await ProcessManager.CreateAsync(
            new CreateProcessModel("FakeExport", null, "groupKey"), DateTime.Now);

        await ProcessRepository.InsertAsync(process, true);

        // Ready -> Exporting
        await ProcessManager.UpdateStateAsync(process, new UpdateProcessStateModel(DateTime.Now, "Exporting"));

        var updateTime = DateTime.Now;

        // Not updating from Exporting to Ready, but add a history for Ready.
        await ProcessManager.UpdateStateAsync(process,
            new UpdateProcessStateModel(updateTime, "Ready", "balalala", ProcessStateFlag.Running, null));

        var histories = await ProcessStateHistoryRepository.GetListAsync(x => x.ProcessId == process.Id);

        histories.Count.ShouldBe(3);
        histories.Count(x => x.StateName == "Ready").ShouldBe(2);
        histories.ShouldContain(x =>
            x.StateName == "Ready" && x.ActionName == "balalala" && x.StateUpdateTime == updateTime);
        histories.ShouldContain(x => x.StateName == "Exporting");
    }

    [Fact]
    public async Task Should_Not_Update_To_Invalid_State()
    {
        var process = await ProcessManager.CreateAsync(
            new CreateProcessModel("FakeExport", null, "groupKey"), DateTime.Now);

        await ProcessRepository.InsertAsync(process, true);

        // Update to an undefined state.
        await Should.ThrowAsync<UndefinedProcessStateException>(() =>
            ProcessManager.UpdateStateAsync(process, new UpdateProcessStateModel(DateTime.Now, "Balalala")));

        // Update to a grandchild state.
        await Should.ThrowAsync<UpdatingToFutureStateException>(() =>
            ProcessManager.UpdateStateAsync(process, new UpdateProcessStateModel(DateTime.Now, "Succeeded")));

        await ProcessManager.UpdateStateAsync(process, new UpdateProcessStateModel(DateTime.Now, "Exporting"));

        // Update to a non-descendant state.
        await Should.ThrowAsync<UpdatingToNonDescendantStateException>(() =>
            ProcessManager.UpdateStateAsync(process,
                new UpdateProcessStateModel(DateTime.Now, "FailedToStartExporting")));
    }

    [Fact]
    public async Task Should_Use_Default_State_If_Not_Set()
    {
        var process = await ProcessManager.CreateAsync(
            new CreateProcessModel("FakeExport", null, "groupKey"), DateTime.Now);

        process.StateFlag.ShouldBe(Options.GetProcessDefinition("FakeExport").GetState("Ready").DefaultStateFlag);

        await ProcessManager.UpdateStateAsync(process, new UpdateProcessStateModel(DateTime.Now, "Exporting"));

        process.StateFlag.ShouldBe(Options.GetProcessDefinition("FakeExport").GetState("Exporting").DefaultStateFlag);
    }
}