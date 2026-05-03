using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.CarModule.Enums;
using CarStore.Shared.C_DTOs.CarModuleDtos;
using Mapster;

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
            ConfigureFeature();
        }
        private static void ConfigureCar()
        {
            TypeAdapterConfig<Car, CarWithDetailsDTO>
                .NewConfig()
                .Map(dest => dest.Features,
         src => src.CarFeatures.Select(cf => cf.Feature))
                .Map(dest=>dest.Drivetrain,
                     src=>src.Drivetrain==Drivetrain.FourWD?"4WD":src.Drivetrain.ToString());

        }
        private static void ConfigureEngine()
        {
            TypeAdapterConfig<Engine, EngineDTO>.NewConfig();
        }
        private static void ConfigureBrand()
        {
            TypeAdapterConfig<Brand, BrandDTO>.NewConfig();
        }
        private static void ConfigureFeature()
        {
            TypeAdapterConfig<Feature, FeatureDTO>.NewConfig();
        }
        private static void ConfigureCategory()
        {
            TypeAdapterConfig<Category, CategoryDTO>.NewConfig();
        }
    }
}
