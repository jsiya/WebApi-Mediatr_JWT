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
}
