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
    public class CarImageConfigurations : IEntityTypeConfiguration<CarImage>
    {
        public void Configure(EntityTypeBuilder<CarImage> builder)
        {
            builder.Property(image => image.ImageUrl)
                .HasMaxLength(2050);

            builder.HasOne<Car>()
                .WithMany(c => c.Images)
                .HasForeignKey(c => c.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
