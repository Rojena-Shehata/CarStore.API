using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.CarModule.Enums;
using CarStore.Services.MappingProfiles;
using CarStore.Shared.C_DTOs.CarModuleDtos;
using Mapster;
using Microsoft.Extensions.Configuration;

namespace CarStore.Services.MapsterConfigurations
{
    public static class MapsterConfig
    {
        public static void Configure(IConfiguration configuration)
        {
            ConfigureCategory();
            ConfigureBrand();
            ConfigureEngine();
            ConfigureCar(configuration);

            ConfigureImage(configuration);
            ConfigureFeature();
        }
        private static void ConfigureCar(IConfiguration configuration)
        {
            TypeAdapterConfig<Car, CarWithDetailsDTO>
                .NewConfig()
                .Map(dest => dest.Features,
         src => src.CarFeatures.Select(cf => cf.Feature))
                .Map(dest => dest.Drivetrain,
                     src => src.Drivetrain == Drivetrain.FourWD ? "4WD" : src.Drivetrain.ToString())
                .Map(dest=>dest.ImageUrls,
                     src=>src.Images.Select(image=>image.ImageUrl.ToImageUrl(configuration)));

        }
        private static void ConfigureEngine()
        {
            TypeAdapterConfig<Engine, EngineDTO>.NewConfig();
        }
        private static void ConfigureImage(IConfiguration configuration)
        {
            TypeAdapterConfig<CarImage, string>.NewConfig()
                .Map(dest=>dest,src=>src.ImageUrl.ToImageUrl(configuration));
            ;
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
