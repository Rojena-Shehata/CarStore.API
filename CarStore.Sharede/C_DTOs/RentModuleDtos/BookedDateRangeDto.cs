using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared
{
    public class BookedDateRangeDto
    {
        public DateTime PickUpDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
