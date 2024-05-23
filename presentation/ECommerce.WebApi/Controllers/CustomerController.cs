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
}
