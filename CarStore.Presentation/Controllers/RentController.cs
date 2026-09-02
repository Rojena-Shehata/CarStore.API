using CarStore.ServicesAbstraction;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<CarWithDetailsDTO>>> GetCarById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _rentService.GetCarByIDAsync(id, userId);
            return HandleResult(result);
        }

    }
}
