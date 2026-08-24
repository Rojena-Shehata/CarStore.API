using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Domain.Entities.OrderModule.PurchaseModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarStore.persistence.Data.Configurations.PurchaseModule
{
    public class FinanceConfiguration : IEntityTypeConfiguration<Finance>
    {
        public void Configure(EntityTypeBuilder<Finance> builder)
        {
            // Primary Key
            builder.HasKey(x => x.PurchaseId);

            // Down Payment
            builder.Property(x => x.DownPayment)
                .IsRequired()
                .HasPrecision(12, 2);

            // Estimated Payment
            builder.Property(x => x.EstimatedPayment)
                .IsRequired()
                .HasPrecision(12, 2);


            // Flat Rate
            builder.Property(x => x.FlatRate)
            .IsRequired()
            .HasPrecision(5, 2);

            // Relationship
            builder.HasOne(x => x.PurchaseRequest)
                .WithOne(x => x.Finance)
                .HasForeignKey<Finance>(x => x.PurchaseId);
        }
    }
}
