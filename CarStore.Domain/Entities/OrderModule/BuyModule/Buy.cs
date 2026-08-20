using CarStore.Domain.Entities.CarModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Domain.Entities.OrderModule.BuyModule.Enums;

namespace CarStore.Domain.Entities.OrderModule.BuyModule
{
    public class Buy : BaseEntity<int>
    {
        public string ReferenceNumber { get; set; } = default!;
        public decimal TotalPrice { get; set; }
        public BuyType BuyType { get; set; }


        //Relationships

        // Many Buys -> One DriverInfo
        //public int DriverInfoId { get; set; }
        ////public DriverInfo Driver { get; set; } = default!;

        // Many Buys -> One Car
        public int CarId { get; set; }
        public Car Car { get; set; } = default!;

        // One Buy -> Zero or One Finance
        public Finance? Finance { get; set; }
    }

}

