using AutoMapper;
using Core.DTOs;
using Core.Models;
using Core.Repositories;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisCache;
using StackExchange.Redis;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
          var products= await _productService.GetProductWithCategory();
            return Ok(products);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var products =await _productService.GetAllAsync();
            var productsDto=_mapper.Map<List<ProductDto>>(products);
            return Ok(productsDto);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var products = await _productService.AddAsync(_mapper.Map<Product>(productDto));
            var productsDto=_mapper.Map<ProductDto>(products);
            return Ok(productsDto);
        }



    }
}
