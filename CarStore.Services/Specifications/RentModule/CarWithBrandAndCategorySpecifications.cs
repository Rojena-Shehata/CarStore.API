using CarStore.Domain.Contracts;
using CarStore.Domain.Entities.CarModule;
using CarStore.Services.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Services
{
   /*  public class CarWithBrandAndCategorySpecifications : BaseSpecification<Car>
     {
     public CarWithBrandAndCategorySpecifications(int id)
         : base(p => p.Id == id)
    {
            AddInclude(query => query.Include(c => c.Brand));
            AddInclude(query => query.Include(c => c.Category));
            AddInclude(query => query.Include(c => c.Images));
        }

     }*/

}
