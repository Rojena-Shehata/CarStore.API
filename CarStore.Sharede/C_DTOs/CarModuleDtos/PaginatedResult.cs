using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Shared.C_DTOs.CarModuleDtos
{
    public record class PaginatedResult<TEntity>(int PageNumber, int PageSize, int Count, IEnumerable<TEntity> Data);

}
