using System.Net;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.CustomerViewModels;

namespace ECommerce.Application.Services;

public interface ICustomerService
{
    Task AddCustomerAsync(CustomerVM customerVm);
    Task<ICollection<CustomerVM?>> GetAllCustomersAsync(PaginationVM paginationVM);
    Task<bool> DeleteCustomerAsync(int customerId);
    Task<HttpStatusCode> UpdateCustomerAsync(int id,CustomerVM customerVm);
}