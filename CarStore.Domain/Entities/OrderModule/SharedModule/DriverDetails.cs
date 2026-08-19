using CarStore.Domain.Entities.OrderModule.RentModule;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? DriversIdNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public DateTime? PrefereedContactTime { get; set; } //
        /*-----------Navigation Properties---------------------*/
        public ICollection<Reservation> reservations { get; set; } = new List<Reservation>();

    }
    
}
