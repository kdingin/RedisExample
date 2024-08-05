using AutoMapper;
using Core.DTOs;
using Core.Models;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductResporitory _productResporitory;
        private readonly IMapper _mapper;
        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IProductResporitory productResporitory, IMapper mapper) : base(repository, unitOfWork)
        {
            _productResporitory = productResporitory;
            _mapper = mapper;
        }

        public async Task<List<ProductWithCategoryDto>> GetProductWithCategory()
        {
            var products=await _productResporitory.GetProductWithCategory();
            var productsDto=_mapper.Map<List<ProductWithCategoryDto>>(products);
            return productsDto;

        }
    }
}
