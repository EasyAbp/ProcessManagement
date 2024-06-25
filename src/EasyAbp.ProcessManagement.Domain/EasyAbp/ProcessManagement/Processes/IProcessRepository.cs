using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.ProcessManagement.Processes;

public interface IProcessRepository : IRepository<Process, Guid>
{
}
