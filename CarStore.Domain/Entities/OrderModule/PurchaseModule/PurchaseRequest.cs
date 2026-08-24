using CarStore.Domain.Entities.CarModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Domain.Entities.OrderModule.PurchaseModule.Enums;

namespace CarStore.Domain.Entities.OrderModule.PurchaseModule
{
    public class PurchaseRequest : BaseEntity<int>
    {
        public string ReferenceNumber { get; set; } = default!;
        public decimal TotalPrice { get; set; }
        public PurchaseRequestType PurchaseRequestType { get; set; }


        //Relationships

        // Many PurchaseRequests -> One DriverInfo
        //public int DriverInfoId { get; set; }
        //public DriverInfo Driver { get; set; } = default!;

        // Many PurchaseRequests -> One Car
        public int CarId { get; set; }
        public Car Car { get; set; } = default!;

        // One PurchaseRequest -> Zero or One Finance
        public Finance? Finance { get; set; }
    }

}

