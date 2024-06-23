using System.Net;
using ECommerce.Application.Repositories;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.CustomerViewModels;

namespace ECommerce.Persistence.Services;

public class CustomerService: ICustomerService
{
    private readonly IReadCustomerRepository _readCustomerRepository;
    private readonly IWriteCustomerRepository _writeCustomerRepository;

    public CustomerService(IReadCustomerRepository readCustomerRepository, IWriteCustomerRepository writeCustomerRepository)
    {
        _readCustomerRepository = readCustomerRepository;
        _writeCustomerRepository = writeCustomerRepository;
    }

    public async Task AddCustomerAsync(CustomerVM customerVm)
    {
        var product = new Customer()
        {
            FirstName = customerVm.FirstName,
            LastName = customerVm.LastName,
            Address = customerVm.Address,
            Email = customerVm.Email,
            Password = customerVm.Password
        };

        await _writeCustomerRepository.AddAsync(product);
        await _writeCustomerRepository.SaveChangeAsync();
    }

    public async Task<ICollection<CustomerVM?>> GetAllCustomersAsync(PaginationVM paginationVM)
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
        return allCustomersVm;
    }

    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        var customer = await _readCustomerRepository.GetByIdAsync(customerId);
        if (customer is null)
            return false;
        await _writeCustomerRepository.DeleteAsync(customerId);
        await _writeCustomerRepository.SaveChangeAsync();
        return true;
    }

    public async Task<HttpStatusCode> UpdateCustomerAsync(int id, CustomerVM customerVm)
    {
        var customer = await _readCustomerRepository.GetByIdAsync(id);
        if (customer is null)
            return HttpStatusCode.NotFound;
        
        customer.FirstName = customerVm.FirstName;
        customer.LastName = customerVm.LastName;
        customer.Address = customerVm.Address;
        customer.Email = customerVm.Email;
        customer.Password = customerVm.Password;
        
        await _writeCustomerRepository.UpdateAsync(customer);
        await _writeCustomerRepository.SaveChangeAsync();
        return HttpStatusCode.OK;
    }
}