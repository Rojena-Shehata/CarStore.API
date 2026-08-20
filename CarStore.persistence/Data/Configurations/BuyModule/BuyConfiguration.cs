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
    public class BuyConfiguration : IEntityTypeConfiguration<Buy>
    {
        public void Configure(EntityTypeBuilder<Buy> builder)
        {
            //primarykey
            builder.HasKey(x => x.Id);

            //refrencetype
            builder.Property(x=>x.ReferenceNumber)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(x=>x.ReferenceNumber)
                .IsUnique();

            //TotalPrice
            builder.Property(x => x.TotalPrice)
                .IsRequired()
                .HasPrecision(12,2);

            //Finance
            builder.HasOne(x => x.Finance)
                .WithOne(x => x.Buy)
                .HasForeignKey<Finance>(x => x.BuyId);

            //DriverInfo
            //builder.HasOne(x => x.Driver)
            //.WithMany(x => x.Buys)
            //.HasForeignKey(x => x.DriverId);

            //Car
            builder.HasOne(x => x.Car)
                .WithMany()
                .HasForeignKey(x => x.CarId);

        }
    }
}
