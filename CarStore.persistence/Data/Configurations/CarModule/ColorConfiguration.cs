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
    internal class ColorConFiguration : IEntityTypeConfiguration<CarColor>
    {
        public void Configure(EntityTypeBuilder<CarColor> carColorBuilder)
        {
            carColorBuilder
                .Property(c => c.Name)
                .HasMaxLength(30);
            carColorBuilder
                .Property(c => c.HexCode)
                .HasMaxLength(7);

            //carColorBuilder.ToTable(builder =>
            //{
            //    builder.HasCheckConstraint("CK_Color_HexCode", "[HexCode] Like [#]______");
            //}  );

        }
    }
}
