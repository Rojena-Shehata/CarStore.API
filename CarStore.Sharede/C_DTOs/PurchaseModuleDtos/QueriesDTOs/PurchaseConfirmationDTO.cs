using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.C_DTOs.PurchaseModuleDtos.QueriesDTOs
{
    public class PurchaseConfirmationDTO
    {
        public int PurchaseId { get; set; }
        public string ReferenceNumber { get; set; } = default!;
        public decimal TotalPrice { get; set; }
        public string PurchaseMethod { get; set; } = default!;
        public CarConfirmationDTO SelectedVehicle { get; set; } = default!;
        public FinanceConfirmationDTO? FinanceDetails { get; set; }
    }

    public class CarConfirmationDTO
    {
        public int CarId { get; set; }
        public string Name { get; set; } = default!;
        public int Year { get; set; }
        public decimal? SalePrice { get; set; }
    }

    public class FinanceConfirmationDTO
    {
        public decimal DownPayment { get; set; }
        public int LoanTerm { get; set; }
        public decimal EstimatedPayment { get; set; }
    }
}