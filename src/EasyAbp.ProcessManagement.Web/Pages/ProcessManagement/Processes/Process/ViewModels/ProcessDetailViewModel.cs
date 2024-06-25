using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace EasyAbp.ProcessManagement.Web.Pages.ProcessManagement.Processes.Process.ViewModels;

public class ProcessDetailViewModel
{
    [Display(Name = "ProcessCorrelationId")]
    public string CorrelationId { get; set; }

    [Display(Name = "ProcessProcessName")]
    public string ProcessName { get; set; }

    [Display(Name = "ProcessStateName")]
    public string StateName { get; set; }

    [Display(Name = "ProcessSubStateName")]
    public string? SubStateName { get; set; }

    [Display(Name = "ProcessStateDetailsText")]
    [TextArea(Rows = 5)]
    public string? StateDetailsText { get; set; }

    [Display(Name = "ProcessStateHistory")]
    [TextArea(Rows = 5)]
    public string? Histories { get; set; }

    [Display(Name = "ProcessCompletionTime")]
    public DateTime? CompletionTime { get; set; }

    public ProcessDetailViewModel(string correlationId, string processName, string stateName, string? subStateName,
        string? stateDetailsText, string? histories, DateTime? completionTime)
    {
        CorrelationId = correlationId;
        ProcessName = processName;
        StateName = stateName;
        SubStateName = subStateName;
        StateDetailsText = stateDetailsText;
        Histories = histories;
        CompletionTime = completionTime;
    }
}