using System.Net;
using ECommerce.Domain.ViewModels;

namespace ECommerce.Application.Services;

public interface ICategoryService
{
    Task AddCategoryAsync(AddCategoryVM categoryVm);
    Task<ICollection<AllCategoryVM>> GetAllCategoriesAsync(PaginationVM paginationVM);
    Task<bool> DeleteCategoryAsync(int categoryId);
    Task<HttpStatusCode> UpdateCategoryAsync(int id,AllCategoryVM updatecategoryVM);
}