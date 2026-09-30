using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.OrderModule.RentModule
{
    public class RentalOffice : BaseEntity<int>
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }

        /*-----------Navigation Properties---------------------*/
        public ICollection<Reservation> PickupRentals { get; set; } = new List<Reservation>();
        public ICollection<Reservation> ReturnRentals { get; set; } = new List<Reservation>();
    }
}
