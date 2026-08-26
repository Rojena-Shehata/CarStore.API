using CarStore.Domain.Entities.OrderModule.PurchaseModule;
using CarStore.Domain.Entities.OrderModule.RentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.OrderModule.SharedModule
{
    public class DriverDetails : BaseEntity<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // Owned Value Object for Address
        public Address Address { get; set; } = new Address();
       
        public string? DriversIdNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public DateTime? PrefereedContactTime { get; set; } //
        /*-----------Navigation Properties---------------------*/
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<PurchaseRequest> PurchaseRequests { get; set; } = new List<PurchaseRequest>();

    }
    
}
