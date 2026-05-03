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
    public class CarFeatureConfiguration : IEntityTypeConfiguration<CarFeature>
    {
        public void Configure(EntityTypeBuilder<CarFeature> builder)
        {
            builder.HasOne(x=>x.Feature)
                .WithMany()
                .HasForeignKey(x=>x.FeatureId);


            builder.HasOne(x => x.Car)
                .WithMany(x => x.CarFeatures)
                .HasForeignKey(x => x.CarId);

            builder.HasKey(x =>new { x.CarId, x.FeatureId });
        }
    }
}
