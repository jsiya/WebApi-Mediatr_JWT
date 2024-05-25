using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IWriteCategoryRepository _writeCategoryRepo;
    private readonly IReadCategoryRepository _readCategoryRepository;

    public CategoryController(IWriteCategoryRepository writeCategoryRepo, IReadCategoryRepository readCategoryRepository)
    {
        _writeCategoryRepo = writeCategoryRepo;
        _readCategoryRepository = readCategoryRepository;
    }

    [HttpPost("AddCategory")]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryVM categoryVM)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = new Category()
        {
            Name = categoryVM.Name,
            Description = categoryVM.Description,
        };

        await _writeCategoryRepo.AddAsync(category);
        await _writeCategoryRepo.SaveChangeAsync();

        return StatusCode(201);
    }
    
    [HttpGet("AllCategories")]
    public async Task<IActionResult> GetAll([FromQuery]PaginationVM paginationVM)
    {
        var categories = await _readCategoryRepository.GetAllAsync();
        var categoryForPage = categories.ToList().
            Skip(paginationVM.Page*paginationVM.PageSize).Take(paginationVM.PageSize).ToList();


        var allCategoryVm = categoryForPage.Select(p => new AllCategoryVM()
        {
            Name = p.Name,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
        }).ToList();

        return Ok(allCategoryVm);
    }
    
    [HttpDelete("DeleteCategoryById/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _readCategoryRepository.GetByIdAsync(id);
        if (category is null)
            return NotFound("Category Not Found");
        await _writeCategoryRepo.DeleteAsync(id);
        await _writeCategoryRepo.SaveChangeAsync();
        return StatusCode(204);
    }

    [HttpPut("UpdateCategoryById/{id}")]
    public async Task<IActionResult> UpdateCategoryById(int id, [FromBody] AllCategoryVM categoryVM)
    {
        var category = await _readCategoryRepository.GetByIdAsync(id);
        if (category is null)
            return NotFound("Category Not Found");
        category.Name = categoryVM.Name;
        category.Description = categoryVM.Description;
        category.ImageUrl = categoryVM.ImageUrl;
        await _writeCategoryRepo.UpdateAsync(category);
        await _writeCategoryRepo.SaveChangeAsync();
        return Ok();
    }
    
    //get category with products
}
