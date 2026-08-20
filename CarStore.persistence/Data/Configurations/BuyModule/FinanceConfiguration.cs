using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Domain.Entities.OrderModule.BuyModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarStore.persistence.Data.Configurations.BuyModule
{
    public class FinanceConfiguration : IEntityTypeConfiguration<Finance>
    {
        public void Configure(EntityTypeBuilder<Finance> builder)
        {
            // Primary Key
            builder.HasKey(x => x.BuyId);

            // Down Payment
            builder.Property(x => x.DownPayment)
                .IsRequired()
                .HasPrecision(12, 2);

            // Estimated Payment
            builder.Property(x => x.EstimatedPayment)
                .IsRequired()
                .HasPrecision(12, 2);

            // Relationship
            builder.HasOne(x => x.Buy)
                .WithOne(x => x.Finance)
                .HasForeignKey<Finance>(x => x.BuyId);
        }
    }
}
