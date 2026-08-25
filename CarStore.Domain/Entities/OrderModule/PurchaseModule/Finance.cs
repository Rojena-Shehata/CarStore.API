using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Domain.Entities.OrderModule.PurchaseModule.Enums;

namespace CarStore.Domain.Entities.OrderModule.PurchaseModule
{
    public class Finance 
    {
        public int PurchaseId { get; set; }
        public decimal DownPayment { get; set; }
        public LoanTerm LoanTerm { get; set; } //in months
        public decimal EstimatedPayment { get; set; }
        public decimal FlatRate { get; set; }


        // Relationships
        public PurchaseRequest PurchaseRequest { get; set; } = default!;
    }
}
