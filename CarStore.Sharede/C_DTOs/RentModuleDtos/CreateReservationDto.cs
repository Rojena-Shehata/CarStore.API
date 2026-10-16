using CarStore.Domain.Entities.OrderModule.RentModule.Enum;
using CarStore.Shared.C_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared
{
    public class CreateReservationDto
    {
        public int CarId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int PickupOfficeId { get; set; }
        public int ReturnOfficeId { get; set; }
        public DriverInfoDto DriverInfo { get; set; } = new();
        


    }
}
