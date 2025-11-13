using inmobiliaryApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace inmobiliaryApi.Infrastructure.Persistence;

public class AppDbContextFactory :  IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {

        var basePath = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(basePath, "../inmobiliaryApi.Api"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
       
        var connectionString = configuration.GetConnectionString("DefaultConnection");
     
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        return new AppDbContext(optionsBuilder.Options);
    }
}