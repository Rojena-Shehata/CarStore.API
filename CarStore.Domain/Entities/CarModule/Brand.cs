using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.CarModule
{
    public class Brand:BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public int SalesCount { get; set; } //remember to increase it when buy or rent a car from

    }
}
