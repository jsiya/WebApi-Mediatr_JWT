using ECommerce.Application.Repositories;
using ECommerce.Application.Services;
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
    private readonly ICategoryService _categoryService;

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

        await _categoryService.AddCategoryAsync(categoryVM);

        return StatusCode(201);
    }
    
    [HttpGet("AllCategories")]
    public async Task<IActionResult> GetAll([FromQuery]PaginationVM paginationVM)
    {
        var allCategoryVm = _categoryService.GetAllCategoriesAsync(paginationVM);

        return Ok(allCategoryVm);
    }
    
    [HttpDelete("DeleteCategoryById/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (await _categoryService.DeleteCategoryAsync(id))
            return StatusCode(204);
        
        return NotFound("Category Not Found");
    }

    [HttpPut("UpdateCategoryById/{id}")]
    public async Task<IActionResult> UpdateCategoryById(int id, [FromBody] AllCategoryVM categoryVM)
    {
        return StatusCode((int)await _categoryService.UpdateCategoryAsync(id, categoryVM));
    }
    
    //get category with products
}
