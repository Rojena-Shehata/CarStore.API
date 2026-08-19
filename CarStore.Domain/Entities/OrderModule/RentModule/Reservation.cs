using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.OrderModule.RentModule.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.OrderModule.RentModule
{
    public class Reservation : BaseEntity<int>
    {
    public DateTime PickupDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal RentPrice { get; set; }
    //public decimal TaxAmount { get; set; }
    //public decimal SubTotal { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Active;
    public string QrToken { get; set; } = string.Empty;
    public int RentnalDurationInDays { get; set; }
    public decimal TotalPrice { get; set; }
    public string BookingReference { get; set; } = string.Empty; // 
    /*----------------------Identity BD-------------*/
    public string UserId { get; set; } = string.Empty;
    /*----------------Navigation-Properties--------------------------*/
    public int CarId { get; set; }
    public Car? Car { get; set; }
    public int PickupOfficeId { get; set; }
    public RentalOffice? PickupOffice { get; set; }
    public int? ReturnOfficeId { get; set; }
    public RentalOffice? ReturnOffice { get; set; }

    public ICollection<RentalPayment> RentalPayments { get; set; } = new List<RentalPayment>();
    //One Reservation may have MANY Rental payment Due to the status may be Pending , Cancelled , Paid
    
    }
}
