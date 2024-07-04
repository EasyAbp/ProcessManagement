using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace EasyAbp.ProcessManagement.Web.Pages.ProcessManagement.Processes.Process.ViewModels;

public class ProcessDetailViewModel
{
    [ReadOnlyInput]
    [Display(Name = "ProcessStateDetailsText")]
    [TextArea(Rows = 5)]
    public string? StateDetailsText { get; set; }

    [ReadOnlyInput]
    [Display(Name = "ProcessProcessName")]
    public string Process { get; }

    [ReadOnlyInput]
    [Display(Name = "ProcessStateName")]
    public string State { get; set; }

    [ReadOnlyInput]
    [Display(Name = "ProcessStateHistory")]
    [TextArea(Rows = 5)]
    public string? Histories { get; set; }

    [ReadOnlyInput]
    [Display(Name = "ProcessCreationTime")]
    public DateTime CreationTime { get; set; }

    [ReadOnlyInput]
    [Display(Name = "ProcessCorrelationId")]
    public string CorrelationId { get; set; }

    public ProcessDetailViewModel(string correlationId, string process, string state, string? stateDetailsText,
        string? histories, DateTime creationTime)
    {
        CorrelationId = correlationId;
        Process = process;
        State = state;
        StateDetailsText = stateDetailsText;
        Histories = histories;
        CreationTime = creationTime;
    }
}