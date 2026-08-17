using CarStore.Shared.C_DTOs.CarModuleDtos;
using CarStore.Shared.C_DTOs.CarModuleDtos.Params;
using CarStore.Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.ServicesAbstraction
{
    public interface ICarService
    {
        Task<Result<PaginatedResult<CarDTO>>> GetAllCarsAsync(CarQueryParams carQueryParams,string userId);
        Task<Result<CarWithDetailsDTO>> GetCarByIDAsync(int carId);
        Task<Result<IEnumerable<CarDTO>>> GetAllFavouritCarsAsync(string search,string userId);
    }
}
