using CarStore.ServicesAbstraction;
using CarStore.Shared.C_DTOs.CarModuleDtos;
using CarStore.Shared.C_DTOs.CarModuleDtos.Params;
using CarStore.Shared.CommonResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Result<PaginatedResult<CarDTO>>>> GetAllCars([FromQuery]CarQueryParams carQueryParams)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _carService.GetAllCarsAsync(carQueryParams,userId);
            return HandleResult(result);
        }
        [Authorize]
        [HttpGet("Favourite")]
        public async Task<ActionResult<Result<PaginatedResult<CarDTO>>>> GetAllFavouriteCars(string? search)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _carService.GetAllFavouritCarsAsync(search,userId);
            return HandleResult(result);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<CarWithDetailsDTO>>> GetCarById(int id)
        {
            var result = await _carService.GetCarByIDAsync(id);
            return HandleResult(result);
        }
    }
}
