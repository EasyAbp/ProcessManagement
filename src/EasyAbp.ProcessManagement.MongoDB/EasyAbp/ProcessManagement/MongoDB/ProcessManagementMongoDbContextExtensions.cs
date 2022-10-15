using Volo.Abp;
using Volo.Abp.MongoDB;

namespace EasyAbp.ProcessManagement.MongoDB;

public static class ProcessManagementMongoDbContextExtensions
{
    public static void ConfigureProcessManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
