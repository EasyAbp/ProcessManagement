using System.Collections.Generic;
using EasyAbp.ProcessManagement.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace EasyAbp.ProcessManagement.Processes;

public class ProcessManagementOptionsTests : ProcessManagementDomainTestBase
{
    [Fact]
    public void Should_Get_Definitions()
    {
        var options = ServiceProvider.GetRequiredService<IOptions<ProcessManagementOptions>>().Value;

        var processDefinition = options.GetProcessDefinition("FakeExport");

        processDefinition.GetState("Ready").ShouldNotBeNull();
        processDefinition.GetState("FailedToStartExporting").ShouldNotBeNull();
        processDefinition.GetState("Exporting").ShouldNotBeNull();
        processDefinition.GetState("ExportFailed").ShouldNotBeNull();
        processDefinition.GetState("Succeeded").ShouldNotBeNull();
        Should.Throw<KeyNotFoundException>(() => processDefinition.GetState("Step10000"));

        processDefinition.InitialStateName.ShouldBe("Ready");
        processDefinition.GetChildrenStateNames("Ready").ToArray()
            .ShouldBeEquivalentTo(new[] { "FailedToStartExporting", "Exporting" });
        processDefinition.GetChildrenStateNames("Exporting").ToArray()
            .ShouldBeEquivalentTo(new[] { "ExportFailed", "Succeeded" });
        processDefinition.GetChildrenStateNames("FailedToStartExporting").ToArray().ShouldBeEmpty();
        processDefinition.GetChildrenStateNames("Succeeded").ToArray().ShouldBeEmpty();
        processDefinition.GetChildrenStateNames("ExportFailed").ToArray().ShouldBeEmpty();
    }

    [Fact]
    public void Should_Not_Add_Duplicate_State()
    {
        var options = ServiceProvider.GetRequiredService<IOptions<ProcessManagementOptions>>().Value;

        var processDefinition = options.GetProcessDefinition("FakeExport");

        Should.Throw<AbpException>(() =>
            processDefinition.AddState(new ProcessStateDefinition("Ready", "Ready", null)));

        Should.Throw<AbpException>(() =>
            processDefinition.AddState(new ProcessStateDefinition("Exporting", "Exporting", "Ready")));
    }
}