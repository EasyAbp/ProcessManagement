using EasyAbp.ProcessManagement.Processes;
using AutoMapper;

namespace EasyAbp.ProcessManagement;

public class ProcessManagementDomainAutoMapperProfile : Profile
{
    public ProcessManagementDomainAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Process, ProcessEto>();
    }
}