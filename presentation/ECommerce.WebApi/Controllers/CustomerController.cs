using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.CustomerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IReadCustomerRepository _readCustomerRepository;
    private readonly IWriteCustomerRepository _writeCustomerRepository;

    public CustomerController(IWriteCustomerRepository writeCustomerRepository, IReadCustomerRepository readCustomerRepository)
    {
        _writeCustomerRepository = writeCustomerRepository;
        _readCustomerRepository = readCustomerRepository;
    }
    
    
    [HttpGet("AllCustomers")]
    public async Task<IActionResult> GetAll([FromQuery]PaginationVM paginationVM)
    {
        var customers = await _readCustomerRepository.GetAllAsync();
        var customerForPage = customers.
            ToList().
            Skip(paginationVM.Page*paginationVM.PageSize).
            Take(paginationVM.PageSize).
            ToList();


        var allCustomersVm = customerForPage.Select(p => new CustomerVM()
        {
            FirstName = p.FirstName,
            LastName = p.LastName,
            Address = p.Address,
            Email = p.Email,
            Password = p.Password
        }).ToList();

        return Ok(allCustomersVm);
    }

    [HttpPost("AddCustomer")]
    public async Task<IActionResult> AddProduct([FromBody] CustomerVM customerVM)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = new Customer()
        {
            FirstName = customerVM.FirstName,
            LastName = customerVM.LastName,
            Address = customerVM.Address,
            Email = customerVM.Email,
            Password = customerVM.Password
        };

        await _writeCustomerRepository.AddAsync(product);
        await _writeCustomerRepository.SaveChangeAsync();

        return StatusCode(201);
    }
    
    [HttpDelete("DeleteCustomerById/{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _readCustomerRepository.GetByIdAsync(id);
        if (customer is null)
            return NotFound("Customer Not Found");
        await _writeCustomerRepository.DeleteAsync(id);
        await _writeCustomerRepository.SaveChangeAsync();
        return StatusCode(204);
    }
    
    [HttpPut("UpdateCustomerById/{id}")]
    public async Task<IActionResult> UpdateCustomerById(int id, [FromBody] CustomerVM customerVm)
    {
        var customer = await _readCustomerRepository.GetByIdAsync(id);
        if (customer is null)
            return NotFound("Customer Not Found");
        
        customer.FirstName = customerVm.FirstName;
        customer.LastName = customerVm.LastName;
        customer.Address = customerVm.Address;
        customer.Email = customerVm.Email;
        customer.Password = customerVm.Password;
        
        await _writeCustomerRepository.UpdateAsync(customer);
        await _writeCustomerRepository.SaveChangeAsync();
        
        return Ok();
    }
    
    
    //customer with orders with products
}
