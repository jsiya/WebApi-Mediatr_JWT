using ECommerce.Application.Repositories;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.CustomerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    
    
    [HttpGet("AllCustomers")]
    public async Task<IActionResult> GetAll([FromQuery]PaginationVM paginationVM)
    {
        var allCustomersVm = await _customerService.GetAllCustomersAsync(paginationVM);

        return Ok(allCustomersVm);
    }

    [HttpPost("AddCustomer")]
    public async Task<IActionResult> AddProduct([FromBody] CustomerVM customerVM)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        await _customerService.AddCustomerAsync(customerVM);
        return StatusCode(201);
    }
    
    [HttpDelete("DeleteCustomerById/{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        if(await _customerService.DeleteCustomerAsync(id))
            return StatusCode(204);
        else
            return NotFound("Customer Not Found");
    }
    
    [HttpPut("UpdateCustomerById/{id}")]
    public async Task<IActionResult> UpdateCustomerById(int id, [FromBody] CustomerVM customerVm)
    {
        return StatusCode((int)await _customerService.UpdateCustomerAsync(id, customerVm));
    }
    
    
    //customer with orders with products
}
