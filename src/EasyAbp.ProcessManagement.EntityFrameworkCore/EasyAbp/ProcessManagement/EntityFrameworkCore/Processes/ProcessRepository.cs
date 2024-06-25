using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.ProcessManagement.Processes;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.Processes;

public class ProcessRepository : EfCoreRepository<IProcessManagementDbContext, Process, Guid>, IProcessRepository
{
    public ProcessRepository(IDbContextProvider<IProcessManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<Process>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}