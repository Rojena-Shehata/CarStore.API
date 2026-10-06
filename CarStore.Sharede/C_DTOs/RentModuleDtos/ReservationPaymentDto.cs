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
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int PickupOfficeId { get; set; }
        public int ReturnOfficeId { get; set; }
        public DriverInfoDto ?DriverInfo { get; set; } 
    }
}
