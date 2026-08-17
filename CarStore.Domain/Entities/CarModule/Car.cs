using CarStore.Domain.Entities.CarModule.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities.CarModule
{
    public class Car:BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public decimal? SalePrice { get; set; }
        public int? MaxSpeed { get; set; }

        public decimal? DailyRentPrice { get; set; }//per day
        public CarMode CarMode { get; set; }//sale or rent
        public string? Overview { get; set; }
        public int Year { get; set; }
        public int SeatsNumber { get; set; }
        public TransmissionType? TransmissionType { get; set; }
        public FuelType FuelType { get; set; }
        public Drivetrain Drivetrain { get; set; }
        //public string Color { get; set; } = default!;
        public Engine Engine { get; set; } = default!;//Owned

        //Relationships
        //many images to 1 car
        public ICollection<CarImage> Images { get; set; } = [];

        //->> 1 Color To many Cars
        public int ColorId { get; set; }
        public CarColor Color { get; set; }

        //->> 1 Brand To many Cars
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        //->> 1 Category To many Cars
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        //Features
        public ICollection<CarFeature> CarFeatures { get; set; } = [];

        //Reviews
        public ICollection<Review> Reviews { get; set; } = [];

        //UserFavouriteCar
        public ICollection<UserFavouriteCar> UserFavouriteCars { get; set; } = [];
    }
}
