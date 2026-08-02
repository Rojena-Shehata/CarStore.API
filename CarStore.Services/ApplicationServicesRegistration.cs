using CarStore.Services.MapsterConfigurations;
using CarStore.ServicesAbstraction;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<ICarService,CarService>();
            MapsterConfig.Configure(configuration);
            return services;
        }

    }
}
