using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public class ProcessStateHistoryRepository : EfCoreRepository<IProcessManagementDbContext, ProcessStateHistory, Guid>, IProcessStateHistoryRepository
{
    public ProcessStateHistoryRepository(IDbContextProvider<IProcessManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<ProcessStateHistory>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}