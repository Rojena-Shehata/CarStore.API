using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared
{
    public class CarDetailsDto
    {
       public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public decimal? RentalPrice { get; set; }
        public string TransmissionType { get; set; } = default!;
        public string? Brand { get; set; }
        public string? DriveTrian { get; set; }
    }
}
