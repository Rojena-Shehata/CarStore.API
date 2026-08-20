using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Domain.Entities.OrderModule.BuyModule.Enums;
using CarStore.Domain.Entities.OrderModule.BuyModule;

namespace CarStore.Domain.Entities.OrderModule.BuyModule
{
    public class Finance 
    {
        public int BuyId { get; set; }
        public decimal DownPayment { get; set; }
        public LoanTerm LoanTerm { get; set; } //in months
        public decimal EstimatedPayment { get; set; }


        // Relationships
        public Buy Buy { get; set; } = default!;
    }
}
