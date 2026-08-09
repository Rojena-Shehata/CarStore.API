using CarStore.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Services.Specifications
{
    public abstract class BaseSpecification<T>:ISpecification<T>
    {
        protected BaseSpecification(Expression<Func<T, bool>>? criteria)
        {
            Criteria = criteria;
        }
       

        public Expression<Func<T, bool>>? Criteria { get; private set; }

        //public List<Expression<Func<T, object>>> Includes { get; } = [];
        public List<Func<IQueryable<T>, IQueryable<T>>> Includes { get; } = new();


        public Expression<Func<T, object>>? OrderByExpression { get; private set; }

        public Expression<Func<T, object>>? OrderByDescendingExpression { get; private set; }
        public bool IsSplitQuery { get; private set; }
        public bool IsNoTracking { get; private set; }
        //Pagination
        public bool IsPaginated { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }

        /////Methods
        //Apply Pagination
        protected void ApplyPagination(int pageIndex, int pageSize)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }
        //include
        protected void AddInclude(Func<IQueryable<T>,IQueryable<T>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        //orderBy
        protected void AddOrderBy(Expression<Func<T, object>> orederByExp)
        {
        }
        
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExp)
        {
            OrderByDescendingExpression = orderByDescExp;
        }
        
        //ApplyNoTracking
        protected void ApplyNoTracking()
        {
            IsNoTracking = true;
        }
        //ApplySplitQuery
        protected void ApplySplitQuery()
        {
            IsSplitQuery = true;
        }
    }
}
