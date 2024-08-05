using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T>GetAsync(int id);
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T,bool>>expression);
        Task AddAsync(T entity);    
        Task AddRangeAsync(IEnumerable<T> entities);    
        void Delete(T entity);
        void Update(T  entity);
        void RemoveRange(IEnumerable<T> entities);  

    }
}
