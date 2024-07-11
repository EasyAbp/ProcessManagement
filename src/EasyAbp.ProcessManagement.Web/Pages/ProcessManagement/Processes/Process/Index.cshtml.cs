using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EasyAbp.ProcessManagement.Web.Options;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace EasyAbp.ProcessManagement.Web.Pages.ProcessManagement.Processes.Process;

public class IndexModel : ProcessManagementPageModel
{
    public ProcessFilterInput ProcessFilter { get; set; } = new();

    public string CustomActions { get; set; } = null!;

    protected ProcessManagementWebOptions Options =>
        LazyServiceProvider.LazyGetRequiredService<IOptions<ProcessManagementWebOptions>>().Value;

    public virtual async Task OnGetAsync()
    {
        if (ProcessFilter.UserName.IsNullOrWhiteSpace())
        {
            ProcessFilter.UserName = CurrentUser.UserName;
        }

        CustomActions = "[" + Options.Actions.Select(x =>
                $"{{text:'{x.DisplayName.Localize(StringLocalizerFactory)}',action:function(data){{{x.TableOnClickCallbackCode}}},visible:function(data){{return data.processName==='{x.ProcessName}'&&data.stateName==='{x.StateName}'&&({x.VisibleCheckCode ?? "true"})}}}}")
            .JoinAsString(",") + "]";

        await Task.CompletedTask;
    }
}

public class ProcessFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "ProcessProcessName")]
    public string? ProcessName { get; set; }

    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "ProcessCorrelationId")]
    public string? CorrelationId { get; set; }

    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "ProcessUserName")]
    public string? UserName { get; set; }

    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "ProcessGroupKey")]
    public string? GroupKey { get; set; }

    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "ProcessStateUpdateTime")]
    public DateTime? StateUpdateTime { get; set; }

    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "ProcessStateName")]
    public string? StateName { get; set; }

    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "ProcessActionName")]
    public string? ActionName { get; set; }

    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "ProcessStateSummaryText")]
    public string? StateSummaryText { get; set; }
}