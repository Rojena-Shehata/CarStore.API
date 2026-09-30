
using CarStore.Domain.Contracts;
using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.CarModule.Enums;
using CarStore.Domain.Entities.OrderModule.PurchaseModule;
using CarStore.Domain.Entities.OrderModule.PurchaseModule.Enums;
using CarStore.Domain.Entities.OrderModule.SharedModule;
using CarStore.ServicesAbstraction;
using CarStore.Shared.C_DTOs.PurchaseModuleDtos;
using CarStore.Shared.C_DTOs.PurchaseModuleDtos.CommandsDTOs;
using CarStore.Shared.C_DTOs.PurchaseModuleDtos.QueriesDTOs;
using CarStore.Shared.CommonResult;

namespace CarStore.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        private const decimal FlatRate = 5.7m;

        public PurchaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PurchaseResponseDTO>> CreatePurchaseAsync(
            PurchaseRequestDTO purchaseRequestDTO,
            string userId)
        {
            // Validate request
            if (purchaseRequestDTO is null)
            {
                return Error.Validation(
                    "Purchase.InvalidRequest",
                    "Purchase request is required.");
            }

            // Validate purchase type
            if (!Enum.IsDefined(
                typeof(PurchaseRequestType),
                purchaseRequestDTO.PurchaseType))
            {
                return Error.Validation(
                    "Purchase.InvalidType",
                    "Purchase type must be Cash or Finance.");
            }

            // Get car
            var carRepository =
                _unitOfWork.GetRepository<Car, int>();

            var car = await carRepository.GetByIdAsync(
                purchaseRequestDTO.CarId);

            if (car is null)
            {
                return Error.NotFound(
                    "Car.NotFound",
                    $"Car with Id {purchaseRequestDTO.CarId} was not found.");
            }

            // Check car availability
            if (car.CarMode != CarMode.Sale)
            {
                return Error.Validation(
                    "Car.NotForSale",
                    $"Car with Id {purchaseRequestDTO.CarId} is not available for purchase.");
            }

            // Check sale price
            if (car.SalePrice is null || car.SalePrice.Value <= 0)
            {
                return Error.Validation(
                    "Car.NoSalePrice",
                    "The car does not have a valid sale price.");
            }

            // Validate finance data before creating entities
            if (purchaseRequestDTO.PurchaseType ==
                PurchaseRequestType.Finance)
            {
                // Validate down payment
                if (!purchaseRequestDTO.DownPayment.HasValue ||
                    purchaseRequestDTO.DownPayment.Value <= 0 ||
                    purchaseRequestDTO.DownPayment.Value >=
                    car.SalePrice.Value)
                {
                    return Error.Validation(
                        "Finance.InvalidDownPayment",
                        "Down payment must be greater than zero and less than the car price.");
                }

                // Validate loan term
                if (!purchaseRequestDTO.LoanTerm.HasValue ||
                    !Enum.IsDefined(
                        typeof(LoanTerm),
                        purchaseRequestDTO.LoanTerm.Value))
                {
                    return Error.Validation(
                        "Finance.InvalidLoanTerm",
                        "Loan term must be 36, 48, 60, or 72 months.");
                }
            }

            // Create driver details
            var driver = new DriverDetails
            {
                FirstName = purchaseRequestDTO.FirstName,
                LastName = purchaseRequestDTO.LastName,
                NationalId = purchaseRequestDTO.NationalId,
                Email = purchaseRequestDTO.Email,
                PhoneNumber = purchaseRequestDTO.PhoneNumber,

                Address = new Address
                {
                    City = purchaseRequestDTO.City,
                    ZipCode = purchaseRequestDTO.ZipCode
                },

                PrefereedContactTime =
                    purchaseRequestDTO.PreferredContactTime
            };

            var driverRepository =
                _unitOfWork.GetRepository<DriverDetails, int>();

            driverRepository.Add(driver);

            // Create purchase request
            var purchaseRequest = new PurchaseRequest
            {
                CarId = purchaseRequestDTO.CarId,
                Car = car,
                Driver = driver,
                PurchaseRequestType = purchaseRequestDTO.PurchaseType,
                TotalPrice = car.SalePrice.Value
            };

            var purchaseRepository =
                _unitOfWork.GetRepository<PurchaseRequest, int>();

            purchaseRepository.Add(purchaseRequest);

            // Create finance record only for Finance purchases
            if (purchaseRequestDTO.PurchaseType ==
                PurchaseRequestType.Finance)
            {
                var finance = new Finance
                {
                    PurchaseRequest = purchaseRequest,
                    DownPayment =
                        purchaseRequestDTO.DownPayment!.Value,
                    LoanTerm =
                        purchaseRequestDTO.LoanTerm!.Value,
                    FlatRate = FlatRate
                };

                // Calculate financed amount
                decimal financedAmount =
                    purchaseRequest.TotalPrice - finance.DownPayment;

                // Convert months to years
                decimal years =
                    (int)finance.LoanTerm / 12m;

                // Calculate flat-rate finance charge
                decimal financeCharge =
                    financedAmount * (finance.FlatRate / 100m) * years;

                // Calculate total repayment
                decimal totalFinanceAmount =
                    financedAmount + financeCharge;

                // Calculate monthly payment
                finance.EstimatedPayment = Math.Round(
                    totalFinanceAmount / (int)finance.LoanTerm,
                    2,
                    MidpointRounding.AwayFromZero);

                purchaseRequest.Finance = finance;
            }

            // Temporary reference number before first save
            purchaseRequest.ReferenceNumber =
                $"TEMP-{Guid.NewGuid():N}";

            await _unitOfWork.SaveChangesAsync();

            // Generate final reference number after obtaining the ID
            purchaseRequest.ReferenceNumber =
                $"PUR-{DateTime.Now.Year}-{purchaseRequest.Id:D5}";

            await _unitOfWork.SaveChangesAsync();

            return new PurchaseResponseDTO
            {
                PurchaseId = purchaseRequest.Id,
                ReferenceNumber = purchaseRequest.ReferenceNumber
            };
        }

        public async Task<Result<PurchaseConfirmationDTO>>
            GetPurchaseConfirmationAsync(int purchaseId)
        {
            // Validate purchase ID
            if (purchaseId <= 0)
            {
                return Error.Validation(
                    "Purchase.InvalidId",
                    "Purchase ID must be greater than zero.");
            }

            // Get purchase request
            var purchaseRepository =
                _unitOfWork.GetRepository<PurchaseRequest, int>();

            var purchase =
                await purchaseRepository.GetByIdAsync(purchaseId);

            if (purchase is null)
            {
                return Error.NotFound(
                    "Purchase.NotFound",
                    $"Purchase with Id {purchaseId} was not found.");
            }

            // Get selected car
            var carRepository =
                _unitOfWork.GetRepository<Car, int>();

            var car =
                await carRepository.GetByIdAsync(purchase.CarId);

            if (car is null)
            {
                return Error.NotFound(
                    "Car.NotFound",
                    $"Car with Id {purchase.CarId} was not found.");
            }

            // Prepare confirmation response
            var confirmation = new PurchaseConfirmationDTO
            {
                PurchaseId = purchase.Id,
                ReferenceNumber = purchase.ReferenceNumber,
                TotalPrice = purchase.TotalPrice,
                PurchaseMethod =
                    purchase.PurchaseRequestType.ToString(),

                SelectedVehicle = new CarConfirmationDTO
                {
                    CarId = car.Id,
                    Name = car.Name,
                    Year = car.Year,
                    SalePrice = car.SalePrice
                }
            };

            // Get finance details only for Finance purchases
            if (purchase.PurchaseRequestType ==
                PurchaseRequestType.Finance)
            {
                var financeRepository =
                    _unitOfWork.GetRepository<Finance, int>();

                var finance =
                    await financeRepository.GetByIdAsync(purchaseId);

                if (finance is null)
                {
                    return Error.NotFound(
                        "Finance.NotFound",
                        $"Finance details for purchase {purchaseId} were not found.");
                }

                confirmation.FinanceDetails =
                    new FinanceConfirmationDTO
                    {
                        DownPayment = finance.DownPayment,
                        LoanTerm = (int)finance.LoanTerm,
                        EstimatedPayment = finance.EstimatedPayment
                    };
            }

            return confirmation;
        }

        public async Task<Result<byte[]>>
            DownloadPurchaseConfirmationAsync(int purchaseId)
        {
            // Validate purchase ID
            if (purchaseId <= 0)
            {
                return Error.Validation(
                    "Purchase.InvalidId",
                    "Purchase ID must be greater than zero.");
            }

            // Get purchase request
            var purchaseRepository =
                _unitOfWork.GetRepository<PurchaseRequest, int>();

            var purchase =
                await purchaseRepository.GetByIdAsync(purchaseId);

            if (purchase is null)
            {
                return Error.NotFound(
                    "Purchase.NotFound",
                    $"Purchase with Id {purchaseId} was not found.");
            }

            // Get selected car
            var carRepository =
                _unitOfWork.GetRepository<Car, int>();

            var car =
                await carRepository.GetByIdAsync(purchase.CarId);

            if (car is null)
            {
                return Error.NotFound(
                    "Car.NotFound",
                    $"Car with Id {purchase.CarId} was not found.");
            }

            // Prepare confirmation data
            var confirmation = new PurchaseConfirmationDTO
            {
                PurchaseId = purchase.Id,
                ReferenceNumber = purchase.ReferenceNumber,
                TotalPrice = purchase.TotalPrice,
                PurchaseMethod =
                    purchase.PurchaseRequestType.ToString(),

                SelectedVehicle = new CarConfirmationDTO
                {
                    CarId = car.Id,
                    Name = car.Name,
                    Year = car.Year,
                    SalePrice = car.SalePrice
                }
            };

            // Get finance details only for Finance purchases
            if (purchase.PurchaseRequestType ==
                PurchaseRequestType.Finance)
            {
                var financeRepository =
                    _unitOfWork.GetRepository<Finance, int>();

                var finance =
                    await financeRepository.GetByIdAsync(purchaseId);

                if (finance is null)
                {
                    return Error.NotFound(
                        "Finance.NotFound",
                        $"Finance details for purchase {purchaseId} were not found.");
                }

                confirmation.FinanceDetails =
                    new FinanceConfirmationDTO
                    {
                        DownPayment = finance.DownPayment,
                        LoanTerm = (int)finance.LoanTerm,
                        EstimatedPayment = finance.EstimatedPayment
                    };
            }

            // Generate PDF
            var pdfService =
                new PurchaseConfirmationPdfService();

            var pdfBytes =
                pdfService.GenerateConfirmationPdf(confirmation);

            return pdfBytes;
        }
    }
}