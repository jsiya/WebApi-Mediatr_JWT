using System.Net;
using ECommerce.Domain.ViewModels;

namespace ECommerce.Application.Services;

public interface IProductService
{
    Task<ICollection<AllProductVM>> GetAllProductsAsync(PaginationVM paginationVM);
    Task<AllProductVM?> GetProductByIdAsync(int productId);
    Task AddProductAsync(AddProductVM productsVM);
    Task<HttpStatusCode> UpdateProductAsync(int id,AddProductVM updateProductVM);
    Task<bool> DeleteProductAsync(int productId);
}