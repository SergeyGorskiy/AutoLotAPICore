using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AutoLotDALCore.EF
{
    public class AutoLotContextFactory : IDesignTimeDbContextFactory<AutoLotContext>
    {
        public AutoLotContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AutoLotContext>();
            var connectionString = @"server=(localdb)\MSSQLLocalDB;database=AutoLotCore;
                                   integrated security=True;MultipleActiveResultSets=True;
                                   App=EntityFramework;";
            optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());

            return new AutoLotContext(optionsBuilder.Options);
        }
    }
}