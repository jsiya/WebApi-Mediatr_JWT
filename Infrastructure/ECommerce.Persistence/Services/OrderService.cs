using System.Net;
using ECommerce.Application.Repositories;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.OrderViewModels;

namespace ECommerce.Persistence.Services;

public class OrderService: IOrderService
{
    private readonly IReadOrderRepository _readOrderRepository;
    private readonly IWriteOrderRepository _writeOrderRepository;
    private readonly IReadProductRepository _productRepository;

    public OrderService(IReadOrderRepository readOrderRepository, IWriteOrderRepository writeOrderRepository, IReadProductRepository productRepository)
    {
        _readOrderRepository = readOrderRepository;
        _writeOrderRepository = writeOrderRepository;
        _productRepository = productRepository;
    }

    public async Task<ICollection<AllOrderVM>> GetAllOrdersAsync(PaginationVM paginationVm)
    {
        var orders = await _readOrderRepository.GetAllAsync();
        var orderForPage = orders.ToList().
            Skip(paginationVm.Page * paginationVm.PageSize).
            Take(paginationVm.PageSize).
            ToList();

        var allOrdersVm = orderForPage.Select(o => new AllOrderVM()
        {
            CustomerEmail = o.Customer.Email,
            OrderDate = o.OrderDate,
            OrderNote = o.OrderNote,
            OrderNumber = o.OrderNumber,
            Total = o.Total
        }).ToList();
        return allOrdersVm;
    }

    public async Task<bool> AddOrderAsync(AddOrderVM orderVm)
    {
        var order = new Order()
        {
            CustomerId = orderVm.CustomerId,
            OrderDate = DateTime.Now,
            OrderNote = orderVm.OrderNote,
            OrderNumber = orderVm.OrderNumber
        };

        foreach (var productVM in orderVm.Products)
        {
            var product = await _productRepository.GetByIdAsync(productVM.ProductId);
            if (product is null)
            {
                return false;
            }

            product.Stock -= productVM.Quantity;
            order.Products.Add(product);

            // Calculate total price
            order.Total += product.Price * productVM.Quantity;
        }
        
        await _writeOrderRepository.AddAsync(order);
        await _writeOrderRepository.SaveChangeAsync();
        return true;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await _readOrderRepository.GetByIdAsync(orderId);
        if (order is null)
            return false;
        await _writeOrderRepository.DeleteAsync(orderId);
        await _writeOrderRepository.SaveChangeAsync();
        return true;
    }

    public async Task<HttpStatusCode> UpdateOrderById(int id, UpdateOrderViewModel orderVm)
    {
        var order = await _readOrderRepository.GetByIdAsync(id);
        if (order is null)
            return HttpStatusCode.NotFound;

        order.OrderNote = orderVm.OrderNote;
        return HttpStatusCode.OK;
    }
}