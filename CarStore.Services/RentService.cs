using CarStore.Domain.Contracts;
using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.CarModule.Enums;
using CarStore.Domain.Entities.OrderModule.RentModule;
using CarStore.Services.Specifications.CarModule;
using CarStore.Services.Specifications.RentModule;
using CarStore.ServicesAbstraction;
using CarStore.Shared;
using CarStore.Shared.CommonResult;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    namespace CarStore.Services
{
    public class RentService :IRentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /*-------------------------------------------------------------------------*/
        public async Task<Result<CarDetailsDto>> GetCarByIDAsync(int carId, string userId)
        {
            var carDto = await _unitOfWork.GetRepository<Car , int>()
           .GetTable() 
           .Where(c => c.Id == carId)
           .Select(c => new
           {
               Mode = c.CarMode, // Rent=2
               Dto = new CarDetailsDto
                   {
                       Id = c.Id,
                       Name = c.Name,
                       ImageUrl = c.Images.Select(img => img.ImageUrl).FirstOrDefault(),
                       RentalPrice = c.DailyRentPrice,
                       TransmissionType = c.TransmissionType.ToString(),
                       Brand = c.Brand.Name,
                       DriveTrian = c.Drivetrain.ToString(),
                   }
                   })
                    .FirstOrDefaultAsync();
                if (carDto is null)
                    return Error.NotFound("Car.NotFound", $"Car with ID {carId} was not found.");
                if (carDto.Mode == CarMode.Sale)
                    return Error.Validation("Car.NotForRent", "This car is for sale and not available for rent.");
                if (carDto is null)
                    return Error.NotFound("Car.NotFound", $"Car with ID {carId} was not found.");
           return carDto.Dto;
        }
        /*----------------------------------------------------------------------------------*/
        public async Task<Result<List<DateTime>>> GetCarUnavailableDatesAsync(int carId , string userId)
        {
            var isCarForRent = await _unitOfWork.GetRepository<Car, int>()
        .GetTable()
        .AnyAsync(c => c.Id == carId && c.CarMode == CarMode.Rent);

            if (!isCarForRent)
                return Error.NotFound("Car.NotFound", $"Car with ID {carId} was not found or is not available for rent.");

            var today = DateTime.UtcNow.Date;

            var spec = new CarActiveRentalsSpecification(carId, today);

            var reservations = await _unitOfWork.GetRepository<Reservation, int>()
                .GetTable(spec)
                .Select(r => new { r.PickupDate, r.ReturnDate })
                .ToListAsync();

            var unavailableDates = new HashSet<DateTime>();

            foreach (var res in reservations)
            {
                for (var date = res.PickupDate.Date; date <= res.ReturnDate.Date; date = date.AddDays(1))
                {
                    if (date >= today)
                    {
                        unavailableDates.Add(date);
                    }
                }
            }

            var unavailabledateResult = unavailableDates.OrderBy(d => d).ToList();
            return Result<List<DateTime>>.Ok(unavailabledateResult, "Unavailable dates retrieved successfully.");
        }
        /*-----------------------------------------------------------------------------------------*/
        public async Task<Result<List<RentalOfficeReadDto>>> GetActiveRentalOfficesAsync(string userId)
        {
            var offices = await _unitOfWork.GetRepository<RentalOffice, int>()
                .GetTable()
                .Where(o => o.IsActive)
                .Select(o => new RentalOfficeReadDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    Address = o.Address
                })
                .ToListAsync();

            return offices;
        }

    }
}
