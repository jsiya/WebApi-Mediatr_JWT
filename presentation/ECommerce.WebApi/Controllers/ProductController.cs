using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Text.Json;
using ECommerce.Application.Services;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("AllProducts")]
    public async Task<IActionResult> GetAll([FromQuery]PaginationVM paginationVM)
    {
        var allProductVm = await _productService.GetAllProductsAsync(paginationVM);

        return Ok(allProductVm);
    }

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody] AddProductVM productVM)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        
        _productService.AddProductAsync(productVM);
        return StatusCode(201);
    }
    
    [HttpDelete("DeleteProductById/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if(await _productService.DeleteProductAsync(id))
            return StatusCode(204);
        return NotFound("Product Not Found");
    }
    
    [HttpPut("UpdateProductById/{id}")]
    public async Task<IActionResult> UpdateProductById(int id, [FromBody] AddProductVM productVm)
    {
        var result = await _productService.UpdateProductAsync(id, productVm);

        return StatusCode((int)result);
    }
    
}
