using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace EasyAbp.ProcessManagement.Web.Pages.ProcessManagement.Processes.Process;

public class IndexModel : ProcessManagementPageModel
{
    public ProcessFilterInput ProcessFilter { get; set; } = new();

    public virtual async Task OnGetAsync()
    {
        if (ProcessFilter.UserName.IsNullOrWhiteSpace())
        {
            ProcessFilter.UserName = CurrentUser.UserName;
        }

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