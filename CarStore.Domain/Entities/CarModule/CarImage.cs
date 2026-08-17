using CarStore.Domain.Entities.CarModule.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.CarModule
{
    public class CarImage:BaseEntity<int>
    {
            public string ImageUrl { get; set; } = default!;

            public CarImageType Type { get; set; }

            public int CarId { get; set; }        
    }
}
