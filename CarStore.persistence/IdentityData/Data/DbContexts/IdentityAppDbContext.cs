using CarStore.Domain.Entities;
using CarStore.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarStore.persistence.IdentityData.Data.DbContexts
{
    public class IdentityAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }

        public IdentityAppDbContext()
        {

        }
        public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            builder.Entity<RefreshToken>().HasIndex(rt => rt.Token).IsUnique();

            builder.Entity<VerificationCode>().HasIndex(vc => new { vc.Email, vc.Purpose });
        }
    }
}