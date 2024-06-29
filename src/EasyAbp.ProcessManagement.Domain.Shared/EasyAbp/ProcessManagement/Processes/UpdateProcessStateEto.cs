using System;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class UpdateProcessStateEto : UpdateProcessStateModel, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public UpdateProcessStateEto()
    {
    }

    public UpdateProcessStateEto(Guid? tenantId, string correlationId, string? actionName, ProcessStateFlag stateFlag,
        string? stateSummaryText, DateTime stateUpdateTime, string stateName, string? stateDetailsText) : base(
        correlationId, actionName, stateFlag, stateSummaryText, stateUpdateTime, stateName, stateDetailsText)
    {
        TenantId = tenantId;
    }
}