using System;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.Processes;

[Serializable]
public class CreateProcessEto : CreateProcessModel, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public CreateProcessEto()
    {
    }

    public CreateProcessEto(Guid? tenantId, string processName, string correlationId, string groupKey,
        string? actionName, ProcessStateFlag stateFlag, string? stateSummaryText, string? stateDetailsText) : base(
        processName, correlationId, groupKey, actionName, stateFlag, stateSummaryText, stateDetailsText)
    {
        TenantId = tenantId;
    }

    public CreateProcessEto(Guid? tenantId, string processName, string correlationId, string groupKey) : base(
        processName, correlationId, groupKey)
    {
        TenantId = tenantId;
    }
}