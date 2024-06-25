using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.Processes.Dtos;
using EasyAbp.ProcessManagement.ProcessStateHistories;
using EasyAbp.ProcessManagement.ProcessStateHistories.Dtos;
using AutoMapper;

namespace EasyAbp.ProcessManagement;

public class ProcessManagementApplicationAutoMapperProfile : Profile
{
    public ProcessManagementApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Process, ProcessDto>();
        CreateMap<ProcessStateHistory, ProcessStateHistoryDto>();
    }
}
