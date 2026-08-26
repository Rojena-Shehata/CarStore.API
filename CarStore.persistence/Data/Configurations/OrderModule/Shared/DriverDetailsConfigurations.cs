using CarStore.Domain.Entities.OrderModule.SharedModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.persistence.Data.Configurations.OrderModule.Shared
{
    public class DriverDetailsConfigurations : IEntityTypeConfiguration<DriverDetails>
    {
        public void Configure(EntityTypeBuilder<DriverDetails> builder)
        {
            builder.ToTable("DriverDetails");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(d => d.LastName).IsRequired().HasMaxLength(50);
            builder.Property(d => d.NationalId).IsRequired().HasMaxLength(14);
            builder.Property(d => d.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(d => d.Email).IsRequired().HasMaxLength(100);

            // Configure Address as Owned Entity
            builder.OwnsOne(d => d.Address, address =>
            {
                address.Property(a => a.StreetAddress)
                       .HasColumnName("StreetAddress")
                       .HasMaxLength(150);

                address.Property(a => a.City)
                       .HasColumnName("City")
                       .HasMaxLength(50);

                address.Property(a => a.ZipCode)
                       .HasColumnName("ZipCode")
                       .HasMaxLength(20);
            });

            builder.Property(d => d.DriversIdNumber).HasMaxLength(50);
            builder.Property(d => d.LicenseNumber).HasMaxLength(50);
        }
    }
}
