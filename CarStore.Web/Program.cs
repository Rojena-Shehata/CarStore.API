using CarStore.Domain.Contracts;
using CarStore.Domain.ExceptionHandling;
using CarStore.persistence.Data.DataSeed;
using CarStore.persistence.Extensions;
using CarStore.persistence.IdentityData.Data.DataSeed;
using CarStore.Services;
using CarStore.Web.Extentions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using CarStore.Services.Extensions;

namespace CarStore.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //mapster
            builder.Services.AddMapster();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerServices();

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataInitializer>("Identity");
            builder.Services.AddKeyedScoped<IDataInitializer, DataInitializer>("NotIdentity");


            var app = builder.Build();

            await app.MigrateIdentityDataAsync();
            await app.MigrateDataAsync();
            await app.SeedIdentityDataAsync();
            await app.SeedDataAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentName}/swagger.json";
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("swagger/v1/swagger.json", "CarStore API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}