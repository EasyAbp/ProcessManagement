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

    public UpdateProcessStateEto(Guid? tenantId, string correlationId, DateTime stateUpdateTime, string stateName,
        string? actionName, ProcessStateFlag stateFlag, string? stateSummaryText)
        : base(stateUpdateTime, stateName, actionName, stateFlag, stateSummaryText)
    {
        TenantId = tenantId;
        CorrelationId = Check.NotNullOrWhiteSpace(correlationId, nameof(correlationId));
    }

    public UpdateProcessStateEto(Guid? tenantId, string correlationId, DateTime stateUpdateTime, string stateName) :
        base(stateUpdateTime, stateName)
    {
        TenantId = tenantId;
        CorrelationId = Check.NotNullOrWhiteSpace(correlationId, nameof(correlationId));
    }
}