using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.C_DTOs.CarModuleDtos
{
    public class CarDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;
        public string? ImageUrl { get; set; } 
        public decimal? Price { get; set; }
        public string PriceLabel { get; set; }
        public double? Review { get; set; }
        public bool IsFavourite { get; set; }

    }
}
