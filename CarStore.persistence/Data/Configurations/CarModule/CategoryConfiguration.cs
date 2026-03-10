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
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> categoryEntityBuilder)
        {
            categoryEntityBuilder
                .Property(category => category.Name)
                .HasMaxLength(50);
        }
    }
}
