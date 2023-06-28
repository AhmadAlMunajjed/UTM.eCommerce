using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace UTM.eCommerce.Data;

public class eCommerceEFCoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public eCommerceEFCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the eCommerceDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<eCommerceDbContext>()
            .Database
            .MigrateAsync();
    }
}
