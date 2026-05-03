using CarStore.ServicesAbstraction;
using Microsoft.Extensions.DependencyInjection;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Services.MapsterConfigurations;

namespace CarStore.Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<ICarService,CarService>();
            MapsterConfig.Configure();
            return services;
        }

    }
}
