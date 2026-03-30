using CarStore.Domain.Entities.CarModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.persistence.Data.Configurations.CarModule
{
    internal class CarConfigurations : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> carEntityBuilder)
        {
            carEntityBuilder
                .OwnsOne(
                    typeof(Engine),
                    "Engine",
                    engineBuilder =>
                    {
                        engineBuilder.Property("Capacity").HasColumnType("Decimal(10,2)");
                    });

            carEntityBuilder
                .Property(c => c.Name)
                .HasMaxLength(200);
            carEntityBuilder
                .Property(c => c.Description)
                .HasMaxLength(500);
           
            carEntityBuilder
                .ToTable(c=>c.HasCheckConstraint(
                    "CK_Car_Year",
                    $"[Year]>=1885 And [Year]<={DateTime.UtcNow.Year}"));

            /////Relationships
            //----------------------
            ////1 color To many Cars
            carEntityBuilder.HasOne(c => c.Color)
                .WithMany()
                .HasForeignKey(c => c.ColorId)
                .OnDelete(DeleteBehavior.NoAction);

            ////1 Brand To many Cars
            carEntityBuilder.HasOne(c => c.Brand)
                .WithMany()
                .HasForeignKey(c => c.BrandId);

            ////1 Category To many Cars
            carEntityBuilder.HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId);

        }
    }
}
