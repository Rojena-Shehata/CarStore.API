using CarStore.Domain.Contracts;
using CarStore.Domain.ExceptionHandling;
using CarStore.persistence.Data.DataSeed;
using CarStore.persistence.Extensions;
using CarStore.persistence.IdentityData.Data.DataSeed;
using CarStore.Web.Extentions;

namespace CarStore.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataInitializer>("Identity");
            builder.Services.AddKeyedScoped<IDataInitializer, DataInitializer>("NotIdentity");

            var app = builder.Build();

            await app.MigrateIdentityDataAsync();
            await app.MigrateDataAsync();
            await app.SeedIdentityDataAsync();
            await app.SeedDataAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}