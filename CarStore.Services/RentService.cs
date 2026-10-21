using CarStore.Domain.Contracts;
using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.CarModule.Enums;
using CarStore.Domain.Entities.OrderModule.RentModule;
using CarStore.Domain.Entities.OrderModule.RentModule.Enum;
using CarStore.Domain.Entities.OrderModule.SharedModule;
using CarStore.Services.Specifications.CarModule;
using CarStore.Services.Specifications.RentModule;
using CarStore.ServicesAbstraction;
using CarStore.Shared;
using CarStore.Shared.C_DTOs;
using CarStore.Shared.CommonResult;
using FluentValidation;
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
        // fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateDateDto> _dateValidator;
        private readonly IValidator<CreateReservationDto> _reservationValidator;

        // constructor
        public RentService(
            IValidator<CreateDateDto> dateValidator,
            IValidator<CreateReservationDto> reservationValidator,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dateValidator = dateValidator;
            _reservationValidator = reservationValidator;
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
        /*-----------------------------------------------------------------------------------------*/
        public async Task<Result<RentalPriceDto>> SelectRentalDate(string userId , CreateDateDto createDateDto)
        {
            // 1. Validate dates
            var validationResult = await _dateValidator.ValidateAsync(createDateDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage))
                    .ToList();

                return Result<RentalPriceDto>.Fail(errors);
            }
            //2. Fetch Car
            var car = await _unitOfWork.GetRepository<Car, int>()
                .GetTable()
                .FirstOrDefaultAsync(c => c.Id == createDateDto.CarId && c.CarMode == CarMode.Rent);

            if (car is null)
            {
                return Error.NotFound("Car.NotFound", $"Car with ID {createDateDto.CarId} was not found or is not available for rent.");
            }

            // 3. Check if Offices exist
            var pickupOfficeExists = await _unitOfWork.GetRepository<RentalOffice, int>()
                .GetTable()
                .AnyAsync(o => o.Id == createDateDto.PickupOfficeId && o.IsActive);

            if (!pickupOfficeExists)
            {
                return Error.NotFound("Office.NotFound", $"Pickup office with ID {createDateDto.PickupOfficeId} is invalid or inactive.");
            }

            var returnOfficeExists = await _unitOfWork.GetRepository<RentalOffice, int>()
                .GetTable()
                .AnyAsync(o => o.Id == createDateDto.ReturnOfficeId && o.IsActive);

            if (!returnOfficeExists)
            {
                return Error.NotFound("Office.NotFound", $"Return office with ID {createDateDto.ReturnOfficeId} is invalid or inactive.");
            }

            // 4. Calculate Duration & Pricing using DailyRentPrice
            var totalDays = (createDateDto.ReturnDate.Date - createDateDto.PickUpDate.Date).Days;
            if (totalDays <= 0) totalDays = 1;

            var dailyPrice = car.DailyRentPrice ?? 0m;
            var rentalTotal = totalDays * dailyPrice;
          

            // 5. Map to DTO
            var resultDto = new RentalPriceDto
            {
                CarId = car.Id,
                RentalDays = totalDays,
                DailyPrice = dailyPrice,
                TotalPrice = rentalTotal,
            };

            return Result<RentalPriceDto>.Ok(resultDto);
        }
        /*------------------------------------------------------------------------------------*/
        public async Task<Result<ReservationPaymentDto>> CreateReservation(string userId, CreateReservationDto createReservationDto)
        {
            // 1. Validate request
            var validationResult = await _reservationValidator
                .ValidateAsync(createReservationDto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => Error.Validation(
                        e.PropertyName,
                        e.ErrorMessage))
                    .ToList();

                return Result<ReservationPaymentDto>.Fail(errors);
            }
            // 2. Fetch car
            var car = await _unitOfWork
                .GetRepository<Car, int>()
                .GetTable()
                .FirstOrDefaultAsync(c =>c.Id == createReservationDto.CarId && c.CarMode == CarMode.Rent);

            if (car is null)
            {
                return Error.NotFound(
                    "Car.NotFound",
                    $"Car with ID {createReservationDto.CarId} was not found or is not available for rent.");
            }
            // 3. Validate pickup office
            var pickupOfficeExists = await _unitOfWork
                .GetRepository<RentalOffice, int>()
                .GetTable()
                .AnyAsync(o =>
                    o.Id == createReservationDto.PickupOfficeId &&
                    o.IsActive);

            if (!pickupOfficeExists)
            {
                return Error.NotFound(
                    "Office.NotFound",
                    "Pickup office is invalid or inactive.");
            }


            // 4. Validate return office
            var returnOfficeExists = await _unitOfWork
                .GetRepository<RentalOffice, int>()
                .GetTable()
                .AnyAsync(o =>
                    o.Id == createReservationDto.ReturnOfficeId &&
                    o.IsActive);

            if (!returnOfficeExists)
            {
                return Error.NotFound(
                    "Office.NotFound",
                    "Return office is invalid or inactive.");
            }
          

            // 5. Check car availability AGAIN
            var hasConflict = await _unitOfWork
                .GetRepository<Reservation, int>()
                .GetTable()
                .AnyAsync(r =>
                    r.CarId == createReservationDto.CarId &&
                    r.Status != ReservationStatus.Cancelled &&
                    createReservationDto.PickupDate < r.ReturnDate &&
                    createReservationDto.ReturnDate > r.PickupDate);

            if (hasConflict)
            {
                return Error.Validation(
                    "Car.NotAvailable",
                    "The car is no longer available for the selected dates.");
            }


            // 6. Calculate rental days AGAIN
            var rentalDays =
                (createReservationDto.ReturnDate.Date -
                 createReservationDto.PickupDate.Date).Days;

            if (rentalDays <= 0)
            {
                rentalDays = 1;
            }


            // 7. Calculate price AGAIN
            var dailyPrice = car.DailyRentPrice ?? 0m;

            var totalPrice = rentalDays * dailyPrice;


            // 8. Create Reservation
            var reservation = new Reservation
            {
                UserId = userId,
                CarId = car.Id,
                PickupDate = createReservationDto.PickupDate,
                ReturnDate = createReservationDto.ReturnDate,
                PickupOfficeId = createReservationDto.PickupOfficeId,
                ReturnOfficeId = createReservationDto.ReturnOfficeId,
                RentPrice = totalPrice,
                Status = ReservationStatus.Pending,
                BookingReference = GenerateBookingReference(),
                // Driver information
                DriverDetails = new DriverDetails
                {
                    FirstName = createReservationDto.DriverInfo!.FirstName,
                    LastName = createReservationDto.DriverInfo.LastName,
                    NationalId = createReservationDto.DriverInfo.NationalId,
                    Email = createReservationDto.DriverInfo.Email,
                    PhoneNumber = createReservationDto.DriverInfo.PhoneNumber,

                    Address = new Address
                    {
                        StreetAddress = createReservationDto.DriverInfo.StreetAddress,
                        City = createReservationDto.DriverInfo.City,
                        ZipCode = createReservationDto.DriverInfo.ZipCode
                    },

                   // DriversIdNumber = createReservationDto.DriverInfo.DriverIdNumber
                }
            };


            // 9. Save Reservation
             _unitOfWork
                .GetRepository<Reservation, int>()
                .Add(reservation);

            await _unitOfWork.SaveChangesAsync();


            return new ReservationPaymentDto
            {
                ReservationId = reservation.Id,
                BookingReference = reservation.BookingReference,
                RentalDays = rentalDays,
                DailyPrice = dailyPrice,
                TotalPrice = totalPrice,
                CarId=car.Id,
                Status=ReservationStatus.Pending
            };
        }
        /*-----------------Helpers--------------------*/
        private string GenerateBookingReference()
        {
            return $"CAR-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}
