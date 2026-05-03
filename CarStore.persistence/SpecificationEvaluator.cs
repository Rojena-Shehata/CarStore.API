using CarStore.Domain.Contracts;
using CarStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.persistence
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity,TKey>(this IQueryable<TEntity> inputQuery,ISpecification<TEntity> spec) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;
            if(spec is not null)
            {
                if (spec.Criteria is not null)
                    query=query.Where(spec.Criteria);

                
                if (spec.OrderByExpression is not null)
                    query = query.OrderBy(spec.OrderByExpression);

                if (spec.OrderByDescendingExpression is not null)
                    query = query.OrderByDescending(spec.OrderByDescendingExpression);

                if (spec.IsPaginated)
                    query = query.Skip(spec.Skip).Take(spec.Take);

                if (spec.Includes is not null&&spec.Includes.Any() )
                    query=spec.Includes.Aggregate(
                        query,
                        (currentQuery,includequery)=>
                                includequery(currentQuery)//
                            );
                //if (spec.Includes is not null&&spec.Includes.Any() )
                //    query=spec.Includes.Aggregate(
                //        query,
                //        (currentQuery,includeExp)=>
                //            currentQuery.Include(includeExp));
              
                if (spec.IsSplitQuery)
                    query = query.AsSplitQuery();

                if(spec.IsNoTracking)
                    query = query.AsNoTracking();



            }
            return query;
        }
    }
}
