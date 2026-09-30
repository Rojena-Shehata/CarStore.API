
using CarStore.Domain.Contracts;
using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.CarModule.Enums;
using CarStore.Domain.Entities.OrderModule.PurchaseModule.Enums;
using CarStore.ServicesAbstraction;
using CarStore.Shared.C_DTOs.FinanceModuleDtos;
using CarStore.Shared.C_DTOs.FinanceModuleDtos.CommandsDTOs;
using CarStore.Shared.CommonResult;

namespace CarStore.Services.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        private const decimal FlatRate = 5.7m;

        public FinanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<FinanceCalculationResponseDTO>> CalculateFinanceAsync(
            FinanceCalculationDTO financeCalculationDTO)
        {
            // Validate loan term
            if (!Enum.IsDefined(typeof(LoanTerm),
                financeCalculationDTO.LoanTerm))
            {
                return Error.Failure(
                    "Finance.InvalidLoanTerm",
                    "Loan term must be 36, 48, 60, or 72 months.");
            }

            // Get car
            var carRepository = _unitOfWork.GetRepository<Car, int>();

            var car = await carRepository.GetByIdAsync(financeCalculationDTO.CarId);

            if (car is null)
            {
                return Error.NotFound(
                    "Car.NotFound",
                    $"Car with Id {financeCalculationDTO.CarId} was not found.");
            }

            // Check car availability
            if (car.CarMode != CarMode.Sale)
            {
                return Error.Failure(
                    "Car.NotForSale",
                    $"Car with Id {financeCalculationDTO.CarId} is not available for purchase.");
            }

            // Check car sale price
            if (car.SalePrice is null || car.SalePrice <= 0)
            {
                return Error.Failure(
                    "Car.NoSalePrice",
                    "The car does not have a valid sale price.");
            }

            // Validate down payment
            if (financeCalculationDTO.DownPayment <= 0 ||
                financeCalculationDTO.DownPayment >= car.SalePrice.Value)
            {
                return Error.Failure(
                    "Finance.InvalidDownPayment",
                    "Down payment must be greater than zero and less than the car price.");
            }

            // Calculate financed amount
            decimal financedAmount =
                car.SalePrice.Value - financeCalculationDTO.DownPayment;

            // Convert loan term from months to years
            decimal years =
                (int)financeCalculationDTO.LoanTerm / 12m;

            // Calculate finance charge using flat rate
            decimal financeCharge =
                financedAmount * (FlatRate / 100m) * years;

            // Calculate total finance amount
            decimal totalFinanceAmount =
                financedAmount + financeCharge;

            // Calculate monthly estimated payment
            decimal estimatedPayment =
                totalFinanceAmount / (int)financeCalculationDTO.LoanTerm;

            // Return calculation result
            return new FinanceCalculationResponseDTO
            {
                EstimatedPayment = Math.Round(
                    estimatedPayment,
                    2,
                    MidpointRounding.AwayFromZero)
            };
        }
    }
}