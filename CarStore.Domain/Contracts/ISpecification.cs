using CarStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Contracts
{
    public interface ISpecification<TEntity>
    {
        Expression<Func<TEntity, bool>>? Criteria { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }
        Expression<Func<TEntity, object>>? OrderByExpression { get; }
        Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; }
        bool IsNoTracking {  get; }
        bool IsSplitQuery {  get; }
        //Pagination
        bool IsPaginated {  get; }
        int Skip {  get; }
        int Take {  get; }
    }
}
