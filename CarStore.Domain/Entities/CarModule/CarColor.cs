using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.CarModule
{
    public class CarColor:BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string HexCode { get; set; }= default!;
    }
}
