using CarStore.Domain.Entities.OrderModule.RentModule.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.C_DTOs
{
    public class ReservationPaymentDto
    {
        public int CarId { get; set; }
        public int ReservationId { get; set; }
        public int RentalDays { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; }
        public string BookingReference { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
