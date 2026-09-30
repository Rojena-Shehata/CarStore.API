using CarStore.Shared.CommonResult;
using CarStore.Shared.C_DTOs.PurchaseModuleDtos.CommandsDTOs;
using CarStore.Shared.C_DTOs.PurchaseModuleDtos.QueriesDTOs;

namespace CarStore.ServicesAbstraction
{
    public interface IPurchaseService
    {
        Task<Result<PurchaseResponseDTO>> CreatePurchaseAsync(
            PurchaseRequestDTO purchaseRequestDTO,
            string userId);

        Task<Result<PurchaseConfirmationDTO>> GetPurchaseConfirmationAsync(
            int purchaseId);

        Task<Result<byte[]>> DownloadPurchaseConfirmationAsync(int purchaseId);
    }
}