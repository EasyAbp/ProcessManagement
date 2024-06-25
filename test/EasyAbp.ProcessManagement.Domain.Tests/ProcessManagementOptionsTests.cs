using System.Collections.Generic;
using System.Linq;
using EasyAbp.ProcessManagement.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace EasyAbp.ProcessManagement;

public class ProcessManagementOptionsTests : ProcessManagementDomainTestBase
{
    [Fact]
    public void Should_Get_Definitions()
    {
        var options = ServiceProvider.GetService<IOptions<ProcessManagementOptions>>();

        options.ShouldNotBeNull();

        var processDefinition = options.Value.GetProcessDefinition("MyDemoProcess");
        
        processDefinition.GetState("Startup").ShouldNotBeNull();
        processDefinition.GetState("Step1").ShouldNotBeNull();
        processDefinition.GetState("Step2").ShouldNotBeNull();
        processDefinition.GetState("Step3").ShouldNotBeNull();
        processDefinition.GetState("Step4").ShouldNotBeNull();
        processDefinition.GetState("Step5").ShouldNotBeNull();
        processDefinition.GetState("Step6").ShouldNotBeNull();
        processDefinition.GetState("Step7").ShouldNotBeNull();
        processDefinition.GetState("Step8").ShouldNotBeNull();
        Should.Throw<KeyNotFoundException>(() => processDefinition.GetState("Step10000"));

        processDefinition.InitialStateName.ShouldBe("Startup");
        processDefinition.GetChildStateNames("Startup").ToArray().ShouldBeEquivalentTo(new[] { "Step1", "Step2" });
        processDefinition.GetChildStateNames("Step1").ToArray().ShouldBeEquivalentTo(new[] { "Step3" });
        processDefinition.GetChildStateNames("Step2").ToArray().ShouldBeEquivalentTo(new[] { "Step3" });
        processDefinition.GetChildStateNames("Step3").ToArray().ShouldBeEquivalentTo(new[] { "Step4", "Step5" });
        processDefinition.GetChildStateNames("Step4").ToArray().ShouldBeEquivalentTo(new[] { "Step6" });
        processDefinition.GetChildStateNames("Step5").ToArray().ShouldBeEquivalentTo(new[] { "Step7" });
        processDefinition.GetChildStateNames("Step6").ToArray().ShouldBeEquivalentTo(new[] { "Step4" });
        processDefinition.GetChildStateNames("Step7").ToArray().ShouldBeEquivalentTo(new[] { "Step8" });
        processDefinition.GetChildStateNames("Step8").ToArray().ShouldBeEquivalentTo(new[] { "Step5" });
    }
}