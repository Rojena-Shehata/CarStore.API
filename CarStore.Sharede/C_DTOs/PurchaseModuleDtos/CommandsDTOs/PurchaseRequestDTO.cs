using CarStore.Domain.Entities.OrderModule.PurchaseModule.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.C_DTOs.PurchaseModuleDtos.CommandsDTOs
{
    public class PurchaseRequestDTO
    {
        public int CarId { get; set; }

        public PurchaseRequestType PurchaseType { get; set; }

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string NationalId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public string City { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        public DateTime PreferredContactTime { get; set; }

        public decimal? DownPayment { get; set; }
        public LoanTerm? LoanTerm { get; set; }
    }
}
