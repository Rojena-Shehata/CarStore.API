using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.C_DTOs
{
    public class DriverInfoDto
    {
        public string ?FirstName { get; set; } 
        public string ?LastName { get; set; } 
        public string ?NationalId { get; set; }
        public string ?Email { get; set; }
        public string ?PhoneNumber { get; set; } 
        public string ?StreetAddress { get; set; }
        public string ?City { get; set; } 
        public string ?ZipCode { get; set; }
        public string ?DriverIdNumber { get; set; } 
    }
}
