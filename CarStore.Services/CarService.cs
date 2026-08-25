using CarStore.Domain.Contracts;
using CarStore.Domain.Entities;
using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.CarModule.Enums;
using CarStore.Domain.Entities.IdentityModule;
using CarStore.Services.MappingProfiles;
using CarStore.Services.Specifications.CarModule;
using CarStore.ServicesAbstraction;
using CarStore.Shared.C_DTOs.CarModuleDtos;
using CarStore.Shared.C_DTOs.CarModuleDtos.CommandsDTOs;
using CarStore.Shared.C_DTOs.CarModuleDtos.Params;
using CarStore.Shared.CommonResult;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CarStore.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public CarService(IUnitOfWork unitOfWork, 
                          IConfiguration configuration,
                          UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _userManager = userManager;
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
                                                ImageUrl = car.Images.OrderBy(image => image.Type)
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
                                                ImageUrl = uc.Car.Images.OrderBy(image => image.Type)
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

        public async Task<Result<CarWithDetailsDTO>> GetCarByIDAsync(int carId, string userId)
        {
            var carSpec = new CarSpecification(carId);
            var car = await _unitOfWork.GetRepository<Car, int>().GetByIdAsync(carSpec);
            if (car is null)
                return Error.NotFound("Car.NotFound", $"Car With Id {carId} is not Found!");

            var carDto = car.Adapt<CarWithDetailsDTO>();//with mapster
            (carDto.Price, carDto.PriceLabel) = GetCarPrice(car);
            if (car.Reviews is not null && car.Reviews.Any())
            {
                carDto.ReviewSummary = new ReviewSummaryDto
                {
                    ReviewsCount = car.Reviews.Count(),
                    AverageRating = car.Reviews.Average(r => r.Rating),
                    Reviews = car.Reviews.Select(review => new ReviewDto
                    {
                        ReviewerImage = review.ReviewerImageUrl is null ? "images/users/user.webp".ToImageUrl(_configuration) :
                                                                           review.ReviewerImageUrl.ToImageUrl(_configuration),
                        ReviewerName = review.ReviewerName,
                        Comment = review.Comment,
                        Date = review.CreatedAt.ToString("dd MMM yyyy"),
                        Rating=review.Rating
                    }).ToList()
                };
            }
            else
            {
                carDto.ReviewSummary = new();
            }
            if (car.UserFavouriteCars is not null)
                carDto.IsFavourite = car.UserFavouriteCars.Any(uc => uc.UserId == userId);
            return carDto;


        }



        ////Commands
        //favourite
        public async Task<Result> AddCarToFavouritesAsync(int carId, string userId)
        {
            var isCarExist = await IsCarExist(carId);

            if (!isCarExist)
                return Error.NotFound("Car.NotFound", $"Car With Id:{carId} IS Not Found!");
            var userFavouriteCarREpo = _unitOfWork.GetRepository<UserFavouriteCar, object>();

            var favouriteCarEntity = await userFavouriteCarREpo.GetTable()
                                                       .FirstOrDefaultAsync(userFavouriteCar => userFavouriteCar.UserId == userId &&
                                                       userFavouriteCar.CarId == carId);

            if (favouriteCarEntity is null)
            {
                userFavouriteCarREpo.Add(new UserFavouriteCar
                {
                    UserId = userId,
                    CarId = carId,
                });
            }
            else
            {
                userFavouriteCarREpo.Delete(favouriteCarEntity);
            }
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();

        }

        //Review
        public async Task<Result> AddCarReviewAsync(AddReviewDTO addReviewDTO, string userId)
        {
            if (addReviewDTO is null)
                return Error.Validation(
                        "DTO.Invalid",
                        "AddReviewDTO cannot be null.");
            if (string.IsNullOrWhiteSpace(userId))
                return Error.Unauthorized(
                    "User.Unauthorized",
                    "User identity could not be determined.");
            var isCarExist = await IsCarExist(addReviewDTO.CarId);
            if (!isCarExist)
                return Error.NotFound(
                    "Car.NotFound",
                    $"Car With Id: {addReviewDTO.CarId} Is Not Found");
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
                    return Error.NotFound(
                        "User.NotFound",
                        $"User With Id: {userId} Is Not Found");
            var reviewRepo = _unitOfWork.GetRepository<Review, object>();
            var isReviewExist = await reviewRepo.GetTable()
                                        .AnyAsync(review => review.ReviewerId == userId &&
                                                       review.CarId == addReviewDTO.CarId);
            if (isReviewExist)
                return Error.Conflict(
                    "Review.AlreadyExists",
                    "You have already added a review for this car.");
            var review = new Review
            {
                CarId = addReviewDTO.CarId,
                ReviewerId = userId,
                ReviewerName = user.FullName,
                ReviewerImageUrl = user.ImageUrl,
                Comment = addReviewDTO.Comment,
                Rating = addReviewDTO.Rating
            };
            reviewRepo.Add(review);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();          
        }

        ////Helper Methods
        private async Task<bool> IsCarExist(int carId)
        {
            return await _unitOfWork.GetRepository<Car, int>()
                                     .GetTable()
                                     .AnyAsync(car => car.Id == carId);
        }
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
