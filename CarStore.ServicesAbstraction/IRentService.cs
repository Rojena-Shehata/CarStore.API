using CarStore.Shared;
using CarStore.Shared.C_DTOs;
using CarStore.Shared.C_DTOs.CarModuleDtos;
using CarStore.Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CarStore.ServicesAbstraction
{
    public interface IRentService
    {
        Task<Result<CarDetailsDto>> GetCarByIDAsync(int carId, string userId);
        Task<Result<List<DateTime>>> GetCarUnavailableDatesAsync(int carId, string userId);
        Task<Result<List<RentalOfficeReadDto>>> GetActiveRentalOfficesAsync(string userId);
        Task<Result<RentalPriceDto>> SelectRentalDate(string userId, CreateDateDto createDateDto);
        Task<Result<ReservationPaymentDto>> CreateReservation(string userId, CreateReservationResponseDto createReservationDto);


    }
}
