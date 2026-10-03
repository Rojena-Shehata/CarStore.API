using CarStore.ServicesAbstraction;
using CarStore.Shared.C_DTOs.PurchaseModuleDtos.CommandsDTOs;
using CarStore.Shared.C_DTOs.PurchaseModuleDtos.QueriesDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarStore.Presentation.Controllers
{
    public class PurchaseController : ApiBaseController
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreatePurchase(
            PurchaseRequestDTO purchaseRequestDTO)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _purchaseService.CreatePurchaseAsync(
                purchaseRequestDTO,
                userId);

            return HandleResult(result);
        }

        [Authorize]
        [HttpGet("/api/purchases/{purchaseId:int}/confirmation")]
        public async Task<ActionResult> GetPurchaseConfirmation(
            int purchaseId)
        {
            var result =
                await _purchaseService.GetPurchaseConfirmationAsync(
                    purchaseId);

            return HandleResult(result);
        }

        [Authorize]
        [HttpGet("/api/purchases/{purchaseId:int}/confirmation/download")]
        public async Task<IActionResult> DownloadPurchaseConfirmation(
            int purchaseId)
        {
            var result =
                await _purchaseService.DownloadPurchaseConfirmationAsync(
                    purchaseId);

            if (!result.IsSucceeded)
                return HandleResult(result);

            const string fileName =
                "Cars_Wroom_Purchase_Confirmation.pdf";

            return File(
                result.Value,
                "application/pdf",
                fileName);
        }
    }
}