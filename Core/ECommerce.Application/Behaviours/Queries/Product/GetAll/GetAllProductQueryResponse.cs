using ECommerce.Domain.ViewModels;

namespace ECommerce.Application.Behaviours.Queries.Product.GetAll;

public class GetAllProductQueryResponse
{
    public ICollection<AllProductVM> Products { get; set; }
}