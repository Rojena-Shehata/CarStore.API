using CarStore.Domain.Entities.OrderModule.RentModule;
using CarStore.Domain.Entities.OrderModule.RentModule.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Services.Specifications.RentModule
{
    public class CarActiveRentalsSpecification : BaseSpecification<Reservation>
    {
        public CarActiveRentalsSpecification(int carId, DateTime fromDate)
        : base(r => r.CarId == carId
                 && r.ReturnDate >= fromDate
                 && r.Status != ReservationStatus.Cancelled)
        {
        }
    }
}
