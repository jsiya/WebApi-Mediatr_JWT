using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Text.Json;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IReadProductRepository _readProductRepo;
    private readonly IWriteProductRepository _writeProductRepo;
    private readonly IReadCategoryRepository _readCategoryRepository;

    public ProductController(IReadProductRepository readProductRepo, IWriteProductRepository writeProductRepo, IReadCategoryRepository readCategoryRepository)
    {
        _readProductRepo = readProductRepo;
        _writeProductRepo = writeProductRepo;
        _readCategoryRepository = readCategoryRepository;
    }

    [HttpGet("AllProducts")]
    public async Task<IActionResult> GetAll([FromQuery]PaginationVM paginationVM)
    {
        var products = await _readProductRepo.GetAllAsync();
        var prodcutForPage = products.ToList().
                    Skip(paginationVM.Page*paginationVM.PageSize).Take(paginationVM.PageSize).ToList();


        var allProductVm = prodcutForPage.Select(p => new AllProductVM()
        {
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            CategoryName = p.Category.Name,
            ImageUrl = p.ImageUrl,
            Stock = p.Stock
        }).ToList();

        return Ok(allProductVm);
    }

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody] AddProductVM productVM)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = new Product()
        {
            Name = productVM.Name,
            Price = productVM.Price,
            Description = productVM.Description,
            CategoryId = productVM.CategoryId,
            Stock = productVM.Stock,
        };

        await _writeProductRepo.AddAsync(product);
        await _writeProductRepo.SaveChangeAsync();

        return StatusCode(201);
    }
    
    [HttpDelete("DeleteProductById/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _readProductRepo.GetByIdAsync(id);
        if (product is null)
            return NotFound("Product Not Found");
        await _writeProductRepo.DeleteAsync(id);
        await _writeProductRepo.SaveChangeAsync();
        return StatusCode(204);
    }
    [HttpPut("UpdateProductById/{id}")]
    public async Task<IActionResult> UpdateProductById(int id, [FromBody] AddProductVM productVm)
    {
        var product = await _readProductRepo.GetByIdAsync(id);
        if (product is null)
            return NotFound("Product Not Found");
        
        var category = await _readCategoryRepository.GetByIdAsync(productVm.CategoryId);
        if (category == null)
            return NotFound("Category Not Found");
        
        product.Name = productVm.Name;
        product.Description = productVm.Description;
        product.Stock = productVm.Stock;
        product.Price = productVm.Price;
        product.CategoryId = productVm.CategoryId;
        await _writeProductRepo.UpdateAsync(product);
        await _writeProductRepo.SaveChangeAsync();
        
        return Ok();
    }
    
}
