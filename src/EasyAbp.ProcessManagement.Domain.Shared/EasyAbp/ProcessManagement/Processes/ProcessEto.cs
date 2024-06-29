using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.MultiTenancy;

namespace EasyAbp.ProcessManagement.Processes;

public class ProcessEto : EntityEto<Guid>, IProcess, IMultiTenant
{
    public Guid? TenantId { get; set; }

    public string ProcessName { get; set; }

    public string CorrelationId { get; set; }

    public string GroupKey { get; set; }

    public string? ActionName { get; set; }

    public ProcessStateFlag StateFlag { get; set; }

    public string? StateSummaryText { get; set; }

    public DateTime StateUpdateTime { get; set; }

    public string StateName { get; set; }
}