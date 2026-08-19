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
            builder.Property(r => r.FirstName).HasMaxLength(50);
            builder.Property(r => r.LastName).HasMaxLength(50);
            builder.Property(r => r.NationalId).HasMaxLength(20);
            builder.Property(r => r.PhoneNumber).HasMaxLength(20);
            builder.Property(r => r.Email).HasMaxLength(100);

        }
    }
}
