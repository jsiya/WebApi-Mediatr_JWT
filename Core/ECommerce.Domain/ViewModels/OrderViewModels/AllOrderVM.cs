namespace ECommerce.Domain.ViewModels.OrderViewModels;

public class AllOrderVM
{
    public string? OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string? OrderNote { get; set; }
    public decimal Total { get; set; }

    // Foreign Key
    public string? CustomerEmail { get; set; }
}