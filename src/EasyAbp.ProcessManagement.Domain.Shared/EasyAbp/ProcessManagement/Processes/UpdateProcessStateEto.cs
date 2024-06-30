using System;
using Volo.Abp;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class UpdateProcessStateEto : UpdateProcessStateModel, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public string CorrelationId { get; set; }

    public UpdateProcessStateEto()
    {
    }

    public UpdateProcessStateEto(Guid? tenantId, string correlationId, string? actionName, ProcessStateFlag stateFlag,
        string? stateSummaryText, DateTime stateUpdateTime, string stateName, string? stateDetailsText) : base(
        actionName, stateFlag, stateSummaryText, stateUpdateTime, stateName, stateDetailsText)
    {
        TenantId = tenantId;
        CorrelationId = Check.NotNullOrWhiteSpace(correlationId, nameof(correlationId));
    }
}