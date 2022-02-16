using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Apps.Admin.DTOs;
using Shop.Api.Apps.Admin.DTOs.CategoryDtos;
using Shop.Api.Apps.Admin.DTOs.ProductDtos;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Api.Apps.Admin.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductPostDto productDto)
        {
            if (await _productRepository.IsExisted((x => x.Id == productDto.CategoryId && !x.IsDeleted)))
                return NotFound();

            Product product = new Product
            {
                Name = productDto.Name,
                SalePrice = productDto.SalePrice,
                CostPrice = productDto.CostPrice,
                DisplayStatus = productDto.DisplayStatus,
                CategoryId = productDto.CategoryId
            };

            await _productRepository.AddAsync(product);
            await _productRepository.CommitAsync();

            return StatusCode(201, product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Product product = await _productRepository.GetAsync((x => x.Id == id && !x.IsDeleted));

            if (product == null) return NotFound();

            ProductGetDto productDto = _mapper.Map<ProductGetDto>(product);

            return Ok(productDto);
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetAll(int page = 1, string search = null)
        {
            var query =_productRepository.GetAll(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Name.Contains(search));

            ListDto<ProductListItemDto> listDto = new ListDto<ProductListItemDto>
            {
                Items = query.Skip((page - 1) * 8).Take(8).Select(x =>
                    new ProductListItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        SalePrice = x.SalePrice,
                        CostPrice = x.CostPrice,
                        DisplayStatus = x.DisplayStatus,
                        Category = new CategoryInProductListItemDto
                        {
                            Id = x.CategoryId,
                            Name = x.Category.Name
                        }
                    }).ToList(),
                TotalCount = query.Count()
            };
            return Ok(listDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductPostDto productDto)
        {
            Product existProduct =await _productRepository.GetAsync(x => x.Id == id);

            if (existProduct == null)
                return NotFound();

            if (existProduct.CategoryId != productDto.CategoryId && !await _productRepository.IsExisted(x => x.Id == productDto.CategoryId && !x.IsDeleted))
                return NotFound();

            existProduct.CategoryId = productDto.CategoryId;
            existProduct.Name = productDto.Name;
            existProduct.SalePrice = productDto.SalePrice;
            existProduct.CostPrice = productDto.CostPrice;
            existProduct.DisplayStatus = productDto.DisplayStatus;

            await _productRepository.CommitAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _productRepository.GetAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

            product.IsDeleted = true;
            product.ModifiedAt = DateTime.UtcNow;

           await _productRepository.CommitAsync();
            return NoContent();
        }
    }
}
