using CarStore.Domain.Entities.OrderModule.SharedModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.persistence.Data.Configurations.OrderModule.Shared
{
    public class DriverDetailsConfigurations : IEntityTypeConfiguration<DriverDetails>
    {
        public void Configure(EntityTypeBuilder<DriverDetails> builder)
        {
        }
    }
}
