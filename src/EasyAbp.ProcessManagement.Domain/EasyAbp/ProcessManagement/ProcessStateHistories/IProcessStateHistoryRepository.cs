using System;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public interface IProcessStateHistoryRepository : IRepository<ProcessStateHistory, Guid>
{
}
