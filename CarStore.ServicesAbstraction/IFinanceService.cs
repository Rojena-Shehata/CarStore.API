using CarStore.Shared.C_DTOs.FinanceModuleDtos.CommandsDTOs;
using CarStore.Shared.C_DTOs.FinanceModuleDtos;
using CarStore.Shared.CommonResult;

namespace CarStore.ServicesAbstraction
{
    public interface IFinanceService
    {
        Task<Result<FinanceCalculationResponseDTO>> CalculateFinanceAsync(
            FinanceCalculationDTO financeCalculationDTO);
    }
}