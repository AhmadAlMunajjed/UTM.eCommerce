using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UTM.eCommerce.Data;

public class eCommerceDbContextFactory : IDesignTimeDbContextFactory<eCommerceDbContext>
{
    public eCommerceDbContext CreateDbContext(string[] args)
    {

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<eCommerceDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"));

        return new eCommerceDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
