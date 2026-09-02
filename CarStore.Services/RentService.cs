using CarStore.Domain.Contracts;
using CarStore.Domain.Entities.CarModule;
using CarStore.Services.Specifications.CarModule;
using CarStore.ServicesAbstraction;
using CarStore.Shared;
using CarStore.Shared.CommonResult;
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
    .Select(c => new CarDetailsDto
    {
        Id = c.Id,
        Name = c.Name,
        ImageUrl = c.Images.Select(img => img.ImageUrl).FirstOrDefault(),
        RentalPrice = c.DailyRentPrice,
        TransmissionType = c.TransmissionType.ToString(), 
        Brand = c.Brand.Name,            
        CategoryName = c.Category.Name   


    })
    .FirstOrDefaultAsync();
            if (carDto is null)
                return Error.NotFound("Car.NotFound", $"Car with ID {carId} was not found.");

            return carDto;
        }

    }
}
