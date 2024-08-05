using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class GenericRepositoryWithCache<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly IDatabase _cache;

        public GenericRepositoryWithCache(AppDbContext context, IDatabase cache)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _cache = cache;
        }
        /// <summary>
        /// //
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            string cacheKey = $"AllItems_{typeof(T).Name}";
            var cachedData = await _cache.StringGetAsync(cacheKey);

            if (!cachedData.IsNullOrEmpty)
            {
                var items = JsonConvert.DeserializeObject<List<T>>(cachedData);
                items.Add(entity);
                await _cache.StringSetAsync(cacheKey, JsonConvert.SerializeObject(items));
            }
            else
            {
                var items = new List<T> { entity };
                await _cache.StringSetAsync(cacheKey, JsonConvert.SerializeObject(items));
            }
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            string cacheKey = $"AllItems_{typeof(T).Name}";

            var cachedData = await _cache.StringGetAsync(cacheKey);

            if (!cachedData.IsNullOrEmpty)
            {
                var items = JsonConvert.DeserializeObject<List<T>>(cachedData);
                return items;
            }
            else
            {
                var items = await _dbSet.AsNoTracking().ToListAsync();

                await _cache.StringSetAsync(cacheKey, JsonConvert.SerializeObject(items));

                return items;
            }
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
