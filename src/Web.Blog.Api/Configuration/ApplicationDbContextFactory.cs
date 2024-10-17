using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Web.Blog.Data;

namespace Web.Blog.Api.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Web.Blog.Api"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
