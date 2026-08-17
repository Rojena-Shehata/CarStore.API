using CarStore.Domain.Contracts;
using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.CarModule.Enums;
using CarStore.Services.MappingProfiles;
using CarStore.Services.Specifications.CarModule;
using CarStore.ServicesAbstraction;
using CarStore.Shared.C_DTOs.CarModuleDtos;
using CarStore.Shared.C_DTOs.CarModuleDtos.Params;
using CarStore.Shared.CommonResult;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CarStore.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public CarService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Result<PaginatedResult<CarDTO>>> GetAllCarsAsync(CarQueryParams carQueryParams, string userId)
        {
            var carRepo = _unitOfWork.GetRepository<Car, int>();
            var spec = new CarSpecification(carQueryParams);

            var carDTOList = await carRepo.GetTable(spec)
                                          .Select(car =>
                                            new CarDTO()
                                            {
                                                Id = car.Id,
                                                Name = car.Name,
                                                ImageUrl = car.Images.OrderBy(image=>image.Type)
                                                                        .Select(image => image.ImageUrl.ToImageUrl(_configuration))
                                                                        .FirstOrDefault() ?? "",
                                                Price = car.CarMode == CarMode.Sale ? car.SalePrice : car.DailyRentPrice,
                                                PriceLabel = car.CarMode == CarMode.Sale ? "For Sale" : "Per Day",

                                                Review = car.Reviews.Any() ?
                                                                          car.Reviews.Average(review => review.Rating)
                                                                          : 0,
                                                IsFavourite = car.UserFavouriteCars.Any(uc => uc.UserId == userId),

                                            }).ToListAsync()
                                              ?? new List<CarDTO>();

            var countSpec = new CountCarsSpecification(carQueryParams);
            var TotalCount = await carRepo.CountAsync(countSpec);
            return
                new PaginatedResult<CarDTO>(
                    carQueryParams.PageNumber,
                    carDTOList.Count,
                    TotalCount,
                    carDTOList
                    );
        }

        public async Task<Result<IEnumerable<CarDTO>>> GetAllFavouritCarsAsync(string search, string userId)
        {
            var carRepo = _unitOfWork.GetRepository<UserFavouriteCar, int>();

            var carDTOList = await carRepo.GetTable()
                                           .Where(uc => uc.UserId == userId &&
                                                (string.IsNullOrWhiteSpace(search) || uc.Car.Name.ToLower().Contains(search.ToLower()))
)
                                          .Select(uc =>
                                            new CarDTO()
                                            {
                                                Id = uc.Car.Id,
                                                Name = uc.Car.Name,
                                                ImageUrl = uc.Car.Images.OrderBy(image=>image.Type)
                                                                        .Select(image => image.ImageUrl.ToImageUrl(_configuration))
                                                                        .FirstOrDefault(),
                                                Price = uc.Car.CarMode == CarMode.Sale ? uc.Car.SalePrice : uc.Car.DailyRentPrice,
                                                PriceLabel = uc.Car.CarMode == CarMode.Sale ? "For Sale" : "Per Day",

                                                Review = uc.Car.Reviews.Any() ?
                                                                          uc.Car.Reviews.Average(review => review.Rating)
                                                                          : 0,
                                                IsFavourite = uc.Car.UserFavouriteCars.Any(uc => uc.UserId == userId),

                                            }).ToListAsync();


            return carDTOList;


        }

        public async Task<Result<CarWithDetailsDTO>> GetCarByIDAsync(int carId)
        {
            var carSpec = new CarSpecification(carId);
            var car = await _unitOfWork.GetRepository<Car, int>().GetByIdAsync(carSpec);
            if (car is null)
                return Error.NotFound("Car.NotFound", $"Car With Id {carId} is not Found!");

            var carDto = car.Adapt<CarWithDetailsDTO>();//with mapster
            (carDto.Price, carDto.PriceLabel) = GetCarPrice(car);
            if (car.Reviews is null || !car.Reviews.Any())
                return carDto;
            carDto.ReviewSummary = new ReviewSummaryDto
            {
                ReviewsCount = car.Reviews.Count(),
                AverageRating = car.Reviews.Select(r => r.Rating).Average()
            };
            return carDto;


        }

        ////Helper Methods
        (decimal? carPrice, string priceLabel) GetCarPrice(Car car)
        {
            if (car == null)
                return (null, string.Empty);

            if (car.CarMode == CarMode.Sale)
                return (car.SalePrice, "For Sale");

            else
                return (car.DailyRentPrice, "Per Day");

        }
    }
}
