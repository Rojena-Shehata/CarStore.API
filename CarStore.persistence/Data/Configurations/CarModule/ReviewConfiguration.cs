using CarStore.Domain.Entities;
using CarStore.Domain.Entities.CarModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarStore.persistence.Data.Configurations.CarModule
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {


            builder.HasKey(x => new { x.UserId, x.CarId });
            

        }
    }
}
