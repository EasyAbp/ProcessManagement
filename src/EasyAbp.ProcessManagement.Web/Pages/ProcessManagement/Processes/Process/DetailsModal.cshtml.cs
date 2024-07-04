using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.Processes.Dtos;
using EasyAbp.ProcessManagement.ProcessStateHistories;
using EasyAbp.ProcessManagement.ProcessStateHistories.Dtos;
using EasyAbp.ProcessManagement.Web.Pages.ProcessManagement.Processes.Process.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.ProcessManagement.Web.Pages.ProcessManagement.Processes.Process;

public class DetailsModalModel : ProcessManagementPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public ProcessDetailViewModel ViewModel { get; set; }

    private readonly IProcessAppService _processAppService;
    private readonly IProcessStateHistoryAppService _processStateHistoryAppService;

    public DetailsModalModel(
        IProcessAppService processAppService,
        IProcessStateHistoryAppService processStateHistoryAppService)
    {
        _processAppService = processAppService;
        _processStateHistoryAppService = processStateHistoryAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _processAppService.GetAsync(Id);

        var histories = await _processStateHistoryAppService.GetListAsync(new ProcessStateHistoryGetListInput
        {
            MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount,
            Sorting = "Id ASC",
            ProcessId = Id,
        });

        var stateString = dto.ActionName is null ? dto.StateDisplayName : $"{dto.StateDisplayName} ({dto.ActionName})";

        var historiesString = histories.Items
            .Select(x =>
                x.ActionName is null
                    ? $"{x.StateUpdateTime}: {x.StateDisplayName}"
                    : $"{x.StateUpdateTime}: {x.StateDisplayName} ({x.ActionName})")
            .JoinAsString(Environment.NewLine);

        ViewModel = new ProcessDetailViewModel(dto.CorrelationId, dto.ProcessDisplayName, stateString,
            dto.StateDetailsText ?? dto.StateSummaryText, historiesString, dto.CreationTime);
    }
}