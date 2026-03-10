using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.CarModule
{
    public class UserFavouriteCars
    {
        public string UserId { get; set; } = default!;
        public int CarId { get; set; }
    }
}
