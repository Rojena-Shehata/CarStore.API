using CarStore.ServicesAbstraction;
using CarStore.Shared;
using CarStore.Shared.C_DTOs;
using CarStore.Shared.C_DTOs.CarModuleDtos;
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
    [ApiController]
    [Route("api/[controller]")]
    public class RentController : ApiBaseController
    {
        private readonly IRentService _rentService;
        public RentController(IRentService rentService)
        {
            _rentService = rentService;
        }
        /*-----------------------------------------------------------------*/
        [Authorize]
        [HttpGet("RentCarById{CarId}")]
        public async Task<ActionResult<Result<CarWithDetailsDTO>>> GetCarById(int CarId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _rentService.GetCarByIDAsync(CarId, userId);
            return HandleResult(result);
        }
        /*-----------------------------------------------------------------*/
        [Authorize]
        [HttpGet("{CarId}/unavailable-dates")]
        public async Task<ActionResult<Result<List<DateTime>>>> GetUnavailableDates(int CarId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _rentService.GetCarUnavailableDatesAsync(CarId, userId);
            return HandleResult(result);
        }
        /*-------------------------------------------------------------------*/
        [Authorize]
        [HttpGet("offices")]
        public async Task<ActionResult<Result<List<RentalOfficeReadDto>>>> GetRentalOffices()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _rentService.GetActiveRentalOfficesAsync(userId);
            return HandleResult(result);
        }
        /*---------------------------------------------------------------*/
        [Authorize]
        [HttpPost("Calculate-RentPrice")]
        public async Task<ActionResult<Result<RentalPriceDto>>> SelectRentalDate(CreateDateDto createDateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _rentService.SelectRentalDate(userId , createDateDto);
            return HandleResult(result);
        }
        /*------------------------------------------------------------*/
        [Authorize]
        [HttpPost ("Create-Reservation")]
        public async Task<ActionResult<Result<ReservationPaymentDto>>> CreateReservation([FromBody] CreateReservationDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _rentService.CreateReservation(userId, dto);
            return HandleResult(result);
        }
    } 
    
}
