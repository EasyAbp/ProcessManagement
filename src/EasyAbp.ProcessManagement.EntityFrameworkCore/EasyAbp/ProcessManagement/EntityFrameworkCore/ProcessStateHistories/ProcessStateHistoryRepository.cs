using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.ProcessStateHistories;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.ProcessStateHistories;

public class ProcessStateHistoryRepository : EfCoreRepository<IProcessManagementDbContext, ProcessStateHistory, Guid>,
    IProcessStateHistoryRepository
{
    public ProcessStateHistoryRepository(IDbContextProvider<IProcessManagementDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public override async Task<IQueryable<ProcessStateHistory>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    public virtual async Task<List<ProcessStateHistory>> GetHistoriesByStateNameAsync(Guid processId, string stateName)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.ProcessId == processId && x.StateName == stateName)
            .ToListAsync();
    }
}