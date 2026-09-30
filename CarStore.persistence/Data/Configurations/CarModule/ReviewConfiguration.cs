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

            builder.HasKey(x => new { x.ReviewerId, x.CarId });

            builder.ToTable("Reviews", table =>
            {
                table.HasCheckConstraint(
                    "CK_Review_Rating",
                    "[Rating] >= 1 AND [Rating] <= 5");
            });

            builder.Property(review => review.ReviewerImageUrl).HasMaxLength(2030);
            builder.Property(review => review.Comment).HasMaxLength(1000);
            builder.Property(review => review.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        }
    }
}
