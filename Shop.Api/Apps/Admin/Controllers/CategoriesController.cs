using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Apps.Admin.DTOs;
using Shop.Api.Apps.Admin.DTOs.CategoryDto;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Api.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository,IMapper mapper )
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryPostDto catregoryDto)
        {
            if (await _categoryRepository.IsExisted((x => x.Name.ToUpper() == catregoryDto.Name.Trim().ToUpper())))
                return StatusCode(409);

            Category category = new Category
            {
                Name = catregoryDto.Name,
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.CommitAsync();

            return StatusCode(201, category);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            //Category category = _context.Categories.Include(x=>x.Products).FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            Category category = await _categoryRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

            if (category == null) return NotFound();

            CategoryGetDto categoryDto = _mapper.Map<CategoryGetDto>(category);

            return Ok(categoryDto);
        }

        [HttpGet("")]
        public IActionResult GetAll(int page = 1)
        {
            var query =  _categoryRepository.GetAll(x => !x.IsDeleted);

            ListDto<CategoryListItemDto> listDto = new ListDto<CategoryListItemDto>
            {
                TotalCount = query.Count(),
                Items = query.Skip((page - 1) * 8).Take(8).Select(x => new CategoryListItemDto { Id = x.Id, Name = x.Name }).ToList()
            };

            return Ok(listDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryPostDto categoryDto)
        {
            Category category = await _categoryRepository.GetAsync(x => x.Id == id && !x.IsDeleted, "Products");

            if (category == null) return NotFound();

            if (await _categoryRepository.IsExisted(x => x.Id != id && x.Name.ToUpper() == categoryDto.Name.Trim().ToUpper()))
                return StatusCode(409);

            category.Name = categoryDto.Name;
            category.ModifiedAt = DateTime.UtcNow;

            await _categoryRepository.CommitAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _categoryRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

            if (category == null) return NotFound();

            category.IsDeleted = true;
            category.ModifiedAt = DateTime.UtcNow;

            await _categoryRepository.CommitAsync();

            return NoContent();
        }
    }
}
