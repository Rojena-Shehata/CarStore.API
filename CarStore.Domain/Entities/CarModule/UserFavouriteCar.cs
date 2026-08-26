using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.CarModule
{
    public class UserFavouriteCar
    {
        public string UserId { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; } = default!;
    }
}
