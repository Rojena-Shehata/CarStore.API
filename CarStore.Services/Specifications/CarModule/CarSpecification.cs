using CarStore.Domain.Entities.CarModule;
using CarStore.Shared.C_DTOs.CarModuleDtos.Params;
using E_Commerce.Services.Specifications.ProductSpecifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Services.Specifications.CarModule
{
    internal class CarSpecification : BaseSpecification<Car>
    {
        public CarSpecification(CarQueryParams carQueryParams) : 
            base(CarSpecificationHelper.CreateCriteria(carQueryParams))
        {
            ApplyPagination(carQueryParams.PageNumber, carQueryParams.PageSize);

            //Sorting
            switch (carQueryParams.Sort)
            {
                case CarSortingOptions.A_Z:
                    AddOrderBy(car => car.Name);
                    break;
                case CarSortingOptions.Z_A:
                    AddOrderByDescending(car => car.Name);
                    break;
                case CarSortingOptions.HighSalesBrands:
                    AddOrderByDescending(car => car.Brand.SalesCount); // if exists
                    break;
            }
        }

       
        //Car By Id
        public CarSpecification(int id) : base(car=>car.Id==id)
        {
            AddInclude(query=>query.Include(c=>c.Brand));
            AddInclude(query => query.Include(c => c.Category));
            AddInclude(query => query.Include(c => c.Color));
            AddInclude(query => query.Include(c => c.CarFeatures).ThenInclude(cf=>cf.Feature));
            
        }
    }
}
