using EasyAbp.ProcessManagement.Processes;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace EasyAbp.ProcessManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProcessToProcessEtoMapper : MapperBase<Process, ProcessEto>
{
    public override partial ProcessEto Map(Process source);
    public override partial void Map(Process source, ProcessEto destination);
}
