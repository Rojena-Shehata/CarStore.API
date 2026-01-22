using CarStore.Domain.Contracts;
using CarStore.persistence.IdentityData.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CarStore.Web.Extentions
{
    public static class WebApplicationRegistration
    {
        public static async Task<WebApplication> MigrateIdentityDataAsync(this WebApplication app)
        {
            var scope=app.Services.CreateScope();
            var dbContextService = scope.ServiceProvider.GetRequiredService<IdentityAppDbContext>();
            if((await dbContextService.Database.GetPendingMigrationsAsync()).Any())
            {
                dbContextService.Database.Migrate();
            }
            return app;
        }


        public static async Task<WebApplication> SeedIdentityDataAsync(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var dataInitializerSevice = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Identity");
            await dataInitializerSevice.InitializeAsync();

            return app;
        }
    }
}
