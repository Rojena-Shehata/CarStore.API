using CarStore.Domain.Entities.CarModule;
using CarStore.Shared.C_DTOs.CarModuleDtos;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Services.MapsterConfigurations
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            ConfigureCategory();
            ConfigureBrand();
            ConfigureEngine();
            ConfigureCar();
        }
        private static void ConfigureCar()
        {
            TypeAdapterConfig<Car, CarWithDetailsDTO>
                .NewConfig();
                
        }
        private static void ConfigureEngine()
        {
            TypeAdapterConfig<Engine, EngineDTO>.NewConfig();
        }
        private static void ConfigureBrand()
        {
            TypeAdapterConfig<Brand, BrandDTO>.NewConfig();
        }
        private static void ConfigureCategory()
        {
            TypeAdapterConfig<Category, CategoryDTO>.NewConfig();
        }
    }
}
