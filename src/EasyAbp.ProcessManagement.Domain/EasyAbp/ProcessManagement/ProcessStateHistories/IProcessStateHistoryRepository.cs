using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public interface IProcessStateHistoryRepository : IRepository<ProcessStateHistory, Guid>
{
    Task<List<ProcessStateHistory>> GetHistoriesByStateNameAsync(Guid processId, string stateName);
}