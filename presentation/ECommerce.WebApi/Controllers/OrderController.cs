using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IReadOrderRepository _readOrderRepository;
    private readonly IWriteOrderRepository _writeOrderRepository;
    private readonly IReadProductRepository _productRepository;

    public OrderController(IReadOrderRepository readOrderRepository, IWriteOrderRepository writeOrderRepository, IReadProductRepository productRepository)
    {
        _readOrderRepository = readOrderRepository;
        _writeOrderRepository = writeOrderRepository;
        _productRepository = productRepository;
    }

    [HttpGet("GetAllOrders")]
    public async Task<IActionResult> GettAll([FromQuery] PaginationVM paginationVM)
    {
        var orders = await _readOrderRepository.GetAllAsync();
        var orderForPage = orders.ToList().
            Skip(paginationVM.Page * paginationVM.PageSize).
            Take(paginationVM.PageSize).
            ToList();

        var allOrdersVm = orderForPage.Select(o => new AllOrderVM()
        {
            CustomerEmail = o.Customer.Email,
            OrderDate = o.OrderDate,
            OrderNote = o.OrderNote,
            OrderNumber = o.OrderNumber,
            Total = o.Total
        }).ToList();
        return Ok(allOrdersVm);
    }


    [HttpPost("AddOrder")]
    public async Task<IActionResult> AddOrder([FromQuery] AddOrderVM orderVm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

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
                ModelState.AddModelError("Products", $"Product with ID {productVM.ProductId} not found.");
                return BadRequest(ModelState);
            }

            product.Stock -= productVM.Quantity;
            order.Products.Add(product);

            // Calculate total price
            order.Total += product.Price * productVM.Quantity;
        }
        
        await _writeOrderRepository.AddAsync(order);
        await _writeOrderRepository.SaveChangeAsync();

        return StatusCode(201);
    }

    [HttpDelete("DeleteOrderById/{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _readOrderRepository.GetByIdAsync(id);
        if (order is null)
            return NotFound("Order Not Found");
        await _writeOrderRepository.DeleteAsync(id);
        await _writeOrderRepository.SaveChangeAsync();
        return StatusCode(204);
    }
    
    
    [HttpPut("UpdateOrderById/{id}")]
    public async Task<IActionResult> UpdateOrderById(int id, [FromBody] UpdateOrderViewModel orderVm)
    {
        var order = await _readOrderRepository.GetByIdAsync(id);
        if (order is null)
            return NotFound("Order Not Found");

        order.OrderNote = orderVm.OrderNote;
        
        return Ok();
    }

    
    
}
