using System.Net;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.OrderViewModels;

namespace ECommerce.Application.Services;

public interface IOrderService
{
    Task<ICollection<AllOrderVM>> GetAllOrdersAsync(PaginationVM paginationVm);
    Task<bool> AddOrderAsync(AddOrderVM  orderVm);
    Task<bool> DeleteOrderAsync(int orderId);
    Task<HttpStatusCode> UpdateOrderById(int id, UpdateOrderViewModel orderVm);
}