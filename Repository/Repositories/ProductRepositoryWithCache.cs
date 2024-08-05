using Core.Models;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ProductRepositoryWithCache : GenericRepositoryWithCache<Product>, IProductResporitory
    {
        public ProductRepositoryWithCache(AppDbContext context, IDatabase cache) : base(context, cache)
        {
        }

        public async Task<List<Product>> GetProductWithCategory()
        {
            string cacheKey = "Products:WithCategory";
            var cachedData = await _cache.StringGetAsync(cacheKey);

            if (cachedData.HasValue)
            {
                return JsonSerializer.Deserialize<List<Product>>(cachedData);
            }

            var productsWithCategory = await _context.Products.Include(x => x.Category).ToListAsync();
            await _cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(productsWithCategory), TimeSpan.FromMinutes(10));
            return productsWithCategory;
        }
    }
}
