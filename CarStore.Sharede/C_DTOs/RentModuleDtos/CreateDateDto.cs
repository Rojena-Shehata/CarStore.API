using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared
{
    public class CreateDateDto
    {
        public int CarId { get; set; }
        public int PickupOfficeId { get; set; }
        public int ReturnOfficeId { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime ReturnDate { get; set; }
        
    }
}
