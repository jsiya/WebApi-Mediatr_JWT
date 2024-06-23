using ECommerce.Application.Repositories;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using ECommerce.Domain.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("GetAllOrders")]
    public async Task<IActionResult> GettAll([FromQuery] PaginationVM paginationVM)
    {
        var allOrdersVm = await _orderService.GetAllOrdersAsync(paginationVM);
        return Ok(allOrdersVm);
    }
    
    [HttpPost("AddOrder")]
    public async Task<IActionResult> AddOrder([FromQuery] AddOrderVM orderVm)
    {
        if (!ModelState.IsValid || await _orderService.AddOrderAsync(orderVm) == false)
            return BadRequest(ModelState);
        return StatusCode(201);
    }

    [HttpDelete("DeleteOrderById/{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        if(await _orderService.DeleteOrderAsync(id))
            return StatusCode(204);
        return NotFound("Order Not Found!");
    }
    
    
    [HttpPut("UpdateOrderById/{id}")]
    public async Task<IActionResult> UpdateOrderById(int id, [FromBody] UpdateOrderViewModel orderVm)
    {
        return StatusCode((int)await _orderService.UpdateOrderById(id, orderVm));
    }

    
    
}
