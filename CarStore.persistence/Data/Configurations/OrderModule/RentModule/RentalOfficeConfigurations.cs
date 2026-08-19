using CarStore.Domain.Entities.OrderModule.RentModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.persistence.Data.Configurations.OrderModule.RentModule
{
    public class RentalOfficeConfigurations : IEntityTypeConfiguration<RentalOffice>
    {
        public void Configure(EntityTypeBuilder<RentalOffice> builder)
        {
        }
    }
}
