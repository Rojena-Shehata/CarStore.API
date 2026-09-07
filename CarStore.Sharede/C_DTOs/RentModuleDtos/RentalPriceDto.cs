using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared
{
    public class RentalPriceDto
    {
        public int CarId { get; set; }
        public int RentalDays { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
