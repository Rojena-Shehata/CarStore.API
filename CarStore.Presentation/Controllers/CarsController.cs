using CarStore.ServicesAbstraction;
using CarStore.Shared.C_DTOs.CarModuleDtos;
using CarStore.Shared.C_DTOs.CarModuleDtos.Params;
using CarStore.Shared.CommonResult;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Presentation.Controllers
{
    public class CarsController:ApiBaseController
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<ActionResult<Result<PaginatedResult<CarDTO>>>> GetAllCars([FromQuery]CarQueryParams carQueryParams)
        {
            var result = await _carService.GetAllCarsAsync(carQueryParams);
            return HandleResult(result);
        }
    }
}
