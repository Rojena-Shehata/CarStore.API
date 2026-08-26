using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.OrderModule.RentModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.persistence.Data.Configurations.OrderModule.RentModule
{
    public class ReservationConfigurations : IEntityTypeConfiguration<Reservation>
    {

        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);

            // Properties Config
            builder.Property(r => r.BookingReference)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(r => r.BookingReference)
                .IsUnique();

            builder.Property(r => r.RentPrice)
                .HasColumnType("decimal(18,2)");

            /*  builder.Property(r => r.TaxAmount)
                  .HasColumnType("decimal(18,2)");*/



            /*--------------- Relationships-------------*/

            // 1. Reservation -> Car (One-to-Many)
            builder.HasOne(r => r.Car)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Reservation -> PickupOffice (One-to-Many)
            builder.HasOne(r => r.PickupOffice)
                .WithMany(o => o.PickupRentals)
                .HasForeignKey(r => r.PickupOfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Reservation -> ReturnOffice (One-to-Many)
            builder.HasOne(r => r.ReturnOffice)
                .WithMany(o => o.ReturnRentals)
                .HasForeignKey(r => r.ReturnOfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4 . DriverDetails
            builder.HasOne(r => r.DriverDetails)
                .WithMany(d => d.Reservations)
                .HasForeignKey(r => r.DriverDetailsId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
    
}
