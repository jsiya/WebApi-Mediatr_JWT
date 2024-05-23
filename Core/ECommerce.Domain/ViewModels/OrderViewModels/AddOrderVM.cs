namespace ECommerce.Domain.ViewModels.OrderViewModels;

public class AddOrderVM
{
    public string? OrderNumber { get; set; }
    public string? OrderNote { get; set; }
    public int CustomerId { get; set; }

    public List<AddProductForOrderVM> Products { get; set; }
}