using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.CarModule
{
    public class Category:BaseEntity<int>
    {
        public string Name { get; set; } = default!;

    }
}
