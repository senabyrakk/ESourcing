using Orders.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Orders.Domain.Repositories.Base
{
    public  interface IBaseRepository<T> where T : EntityBase,new()
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
                                 Func<IQueryable<T>,IOrderedQueryable<T>> orderBy = null,
                                 string includeString = null,
                                 bool disableTracking = true);
        Task<T> GetAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
