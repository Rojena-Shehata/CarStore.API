using CarStore.Domain.Entities.OrderModule.RentModule.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.OrderModule.RentModule
{
    public class RentalPayment : BaseEntity<int>
    {
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string StripePaymentIntentId { get; set; } = string.Empty;  //GUID
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow; //Iautitable
        /*----------------------Identity BD-------------*/
        public string UserId { get; set; } = string.Empty;
        /*-----------------Navigation-property------------*/
        public Reservation Reservation { get; set; } = null!;
        public int ReservationId { get; set; }
    }

    
   
}
