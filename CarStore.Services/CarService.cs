using CarStore.Domain.Contracts;
using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.CarModule.Enums;
using CarStore.Services.Specifications.CarModule;
using CarStore.ServicesAbstraction;
using CarStore.Shared.C_DTOs.CarModuleDtos;
using CarStore.Shared.C_DTOs.CarModuleDtos.Params;
using CarStore.Shared.CommonResult;
using Mapster;

namespace CarStore.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<CarDTO>>> GetAllCarsAsync(CarQueryParams carQueryParams)
        {
            var carRepo = _unitOfWork.GetRepository<Car, int>();
            var spec = new CarSpecification(carQueryParams);

            var cars = await carRepo.GetAllAsync(spec);
            var carDTOList = cars?.Select(car =>
            {
                var priceInfo = GetCarPrice(car);
                return new CarDTO()
                {
                    Id = car.Id,
                    Name = car.Name,
                    Price = priceInfo.carPrice,
                    PriceLabel = priceInfo.priceLabel
                };
            }).ToList() ?? new List<CarDTO>();
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

        public async Task<Result<CarWithDetailsDTO>> GetCarByID(int carId)
        {
            var carSpec = new CarSpecification(carId);
            var car=await _unitOfWork.GetRepository<Car, int>().GetByIdAsync(carSpec);
            if (car is null)
                return Error.NotFound("Car.NotFound", $"Car With Id {carId} is not Found!");

           var carDto= car.Adapt<CarWithDetailsDTO>();//with mapster
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
