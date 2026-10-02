using CarStore.ServicesAbstraction;
using CarStore.Shared.C_DTOs.FinanceModuleDtos;
using CarStore.Shared.C_DTOs.FinanceModuleDtos.CommandsDTOs;
using CarStore.Shared.CommonResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarStore.Presentation.Controllers
{
    public class FinanceController : ApiBaseController
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        [Authorize]
        [HttpPost("calculate")]
        public async Task<ActionResult> CalculateFinance(
            FinanceCalculationDTO financeCalculationDTO)
        {
            var result = await _financeService.CalculateFinanceAsync(
                financeCalculationDTO);

            return HandleResult(result);
        }
    }
}