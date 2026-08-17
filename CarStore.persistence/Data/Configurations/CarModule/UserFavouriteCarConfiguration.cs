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
    public class UserFavouriteCarConfiguration : IEntityTypeConfiguration<UserFavouriteCar>
    {
        public void Configure(EntityTypeBuilder<UserFavouriteCar> builder)
        {
            builder.HasOne(uc => uc.Car)
                .WithMany(car=>car.UserFavouriteCars)
                .HasForeignKey(uc =>uc.CarId);

            builder.HasKey(uc => new { uc.UserId, uc.CarId });
                
                
        }
    }
}
