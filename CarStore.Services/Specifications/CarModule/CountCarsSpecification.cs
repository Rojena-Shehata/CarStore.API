using CarStore.Domain.Entities.CarModule;
using CarStore.Shared.C_DTOs.CarModuleDtos.Params;
using E_Commerce.Services.Specifications.ProductSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Services.Specifications.CarModule
{
    public class CountCarsSpecification:BaseSpecification<Car>
    {
        public CountCarsSpecification(CarQueryParams carQueryParams):base(CarSpecificationHelper.CreateCriteria(carQueryParams))
        {
        }
    }
}
