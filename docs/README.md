# ProcessManagement

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2FEasyAbp%2FProcessManagement%2Fmain%2FDirectory.Build.props)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/EasyAbp.ProcessManagement.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.ProcessManagement.Domain.Shared)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.ProcessManagement.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.ProcessManagement.Domain.Shared)
[![Discord online](https://badgen.net/discord/online-members/xyg8TrRa27?label=Discord)](https://discord.gg/xyg8TrRa27)
[![GitHub stars](https://img.shields.io/github/stars/EasyAbp/ProcessManagement?style=social)](https://www.github.com/EasyAbp/ProcessManagement)

An ABP module that helps define and track business processes.

![Notifications](/docs/images/notifications.gif)

![ProcessDetails](/docs/images/process-details.jpg)

## Installation

1. Install the following NuGet packages. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/docs/How-To.md#add-nuget-packages))

    * EasyAbp.ProcessManagement.Application
    * EasyAbp.ProcessManagement.Application.Contracts
    * EasyAbp.ProcessManagement.Domain
    * EasyAbp.ProcessManagement.Domain.Shared
    * EasyAbp.ProcessManagement.EntityFramework
    * EasyAbp.ProcessManagement.HttpApi
    * EasyAbp.ProcessManagement.HttpApi.Client
    * EasyAbp.ProcessManagement.Web

2. Add `DependsOn(typeof(Abp.ProcessManagementXxxModule))` attribute to configure the module dependencies. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/docs/How-To.md#add-module-dependencies))

## Usage

1. Define a process and states. For idempotency, stages can only transition from their father state.
   ```csharp
   Configure<ProcessManagementOptions>(options =>
   {
       var definition = new ProcessDefinition("MyProcess", new LocalizableString("Process:MyProcess"))
           .AddState(new ProcessStateDefinition(
               name: "Ready",
               displayName: new LocalizableString("State:Ready"),
               fatherStateName: null,
               defaultStateFlag: ProcessStateFlag.Information))
           .AddState(new ProcessStateDefinition(
               name: "Failed",
               displayName: new LocalizableString("State:Failed"),
               fatherStateName: "Ready",
               defaultStateFlag: ProcessStateFlag.Failure))
           .AddState(new ProcessStateDefinition(
               name: "Succeeded",
               displayName: new LocalizableString("State:Succeeded"),
               fatherStateName: "Ready",
               defaultStateFlag: ProcessStateFlag.Success));

       options.AddOrUpdateProcessDefinition(definition);
   });
   ```
2. Now you can create a process and update its state anytime, anywhere.
   ```csharp
   /*
    * Only a specific user can view this process. You can implement IUserGroupContributor yourself
    * to specify more than one user to view this process, e.g. OrganizationUnitUserGroupContributor.
    */
   var groupKey = await _userIdUserGroupContributor.CreateGroupKeyAsync(adminUser!.Id.ToString());

   // Create a process.
   var process1 = await _processManager.CreateAsync(
       new CreateProcessModel(
           processName: "MyProcess",
           correlationId: null, // If null, this value will be auto-set to the value of the Id of the Process entity.
           groupKey: groupKey
       ), Clock.Now);

   // When your process is moving forward.
   await _processManager.UpdateStateAsync(process1,
       new UpdateProcessStateModel(
           stateUpdateTime: Clock.Now,
           stateName: "Succeeded"));

   // Or add more information.
   await _processManager.UpdateStateAsync(process1,
       new UpdateProcessStateModel(
           stateUpdateTime: Clock.Now,
           stateName: "Succeeded",
           actionName: "Export is done!",
           stateFlag: ProcessStateFlag.Success, // If null, use the default value you defined.
           stateSummaryText: "Congratulations! Export successful."));
   ```

   ![IProcessStateCustom](/docs/images/IProcessStateCustom.jpg)
3. If you want, you can create user actions for states.
   ```csharp
   Configure<ProcessManagementWebOptions>(options =>
   {
       options.Actions.Add(new ProcessStateActionDefinition(
           processName: "MyProcess",
           stateName: "Failed",
           displayName: new LocalizableString("Action:Ping"),
           tableOnClickCallbackCode: "window.alert('Pong')", // Not shown in process list page if null.
           offcanvasOnClickCallbackCode: "window.alert('Pong')", // Not shown in notifications offcanvas if null.
           visibleCheckCode: "abp.auth.isGranted('MyProject.MyProcess.Ping')"));
   });
   ```

   ![Actions1](/docs/images/actions1.jpg)

   ![Actions2](/docs/images/actions2.jpg)

## Road map

- [x] Use websocket to update notifications. [#30](https://github.com/EasyAbp/ProcessManagement/issues/30)
- [ ] Better process details modal.