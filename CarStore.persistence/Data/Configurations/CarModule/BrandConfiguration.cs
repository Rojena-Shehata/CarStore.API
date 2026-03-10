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
    internal class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> brandEntityBuilder)
        {
            brandEntityBuilder
                .Property(brand => brand.Name)
                .HasMaxLength(50);
        }
    }
}
