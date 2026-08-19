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
    public class RentalPaymentConfigurations : IEntityTypeConfiguration<RentalPayment>
    {
        public void Configure(EntityTypeBuilder<RentalPayment> builder)
        {
            builder.ToTable("RentalPayments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentStatus)
                .HasMaxLength(50);
            builder.Property(p => p.StripePaymentIntentId)
                .IsRequired()
                .HasMaxLength(150);

            //  RentalPayment -> Reservation (One-to-Many)
            builder
             .HasOne(p => p.Reservation)
             .WithMany(r => r.RentalPayments)
             .HasForeignKey(p => p.ReservationId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
