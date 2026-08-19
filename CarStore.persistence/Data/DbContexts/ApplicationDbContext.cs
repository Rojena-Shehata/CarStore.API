using CarStore.Domain.Entities;
using CarStore.Domain.Entities.CarModule;
using CarStore.Domain.Entities.OrderModule.RentModule;
using CarStore.Domain.Entities.OrderModule.SharedModule;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CarStore.persistence.Data.DbContexts
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<CarFeature> CarFeatures { get; set; }
        public DbSet<CarColor> Colors { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        /*----------OrderSharedModule----------*/
        public DbSet<DriverDetails> DriverDetails { get; set; }
        /*----------Rentmodule-----------------*/
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<RentalOffice> RentalOffices { get; set; }
        public DbSet<RentalPayment> RentalPayments { get; set; }
    }
}
