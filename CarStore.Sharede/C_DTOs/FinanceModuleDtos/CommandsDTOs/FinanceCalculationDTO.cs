using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Domain.Entities.OrderModule.PurchaseModule.Enums;


namespace CarStore.Shared.C_DTOs.FinanceModuleDtos.CommandsDTOs
{
    public class FinanceCalculationDTO
    {
        public int CarId { get; set; }
        public decimal DownPayment { get; set; }
        public LoanTerm LoanTerm { get; set; }

    }
}
