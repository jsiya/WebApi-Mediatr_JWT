using System.Net;
using ECommerce.Application.Repositories;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;

namespace ECommerce.Persistence.Services;

public class CategoryService: ICategoryService
{
    private readonly IWriteCategoryRepository _writeCategoryRepo;
    private readonly IReadCategoryRepository _readCategoryRepository;

    public CategoryService(IWriteCategoryRepository writeCategoryRepo, IReadCategoryRepository readCategoryRepository)
    {
        _writeCategoryRepo = writeCategoryRepo;
        _readCategoryRepository = readCategoryRepository;
    }

    public async Task AddCategoryAsync(AddCategoryVM categoryVm)
    {
        var category = new Category()
        {
            Name = categoryVm.Name,
            Description = categoryVm.Description,
        };

        await _writeCategoryRepo.AddAsync(category);
        await _writeCategoryRepo.SaveChangeAsync();
    }

    public async Task<ICollection<AllCategoryVM>> GetAllCategoriesAsync(PaginationVM paginationVM)
    {
        var categories = await _readCategoryRepository.GetAllAsync();
        var categoryForPage = categories.ToList().
            Skip(paginationVM.Page*paginationVM.PageSize).Take(paginationVM.PageSize).ToList();


        var allCategoryVm = categoryForPage.Select(p => new AllCategoryVM()
        {
            Name = p.Name,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
        }).ToList();
        return allCategoryVm;
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        var category = await _readCategoryRepository.GetByIdAsync(categoryId);
        if(category is null)
            return false;
        else
        {
            await _writeCategoryRepo.DeleteAsync(categoryId);
            await _writeCategoryRepo.SaveChangeAsync();
            return true;
        }
    }

    public async Task<HttpStatusCode> UpdateCategoryAsync(int id, AllCategoryVM updatecategoryVM)
    {
        var category = await _readCategoryRepository.GetByIdAsync(id);
        if (category is null)
            return HttpStatusCode.NotFound;
        category.Name = updatecategoryVM.Name;
        category.Description = updatecategoryVM.Description;
        category.ImageUrl = updatecategoryVM.ImageUrl;
        await _writeCategoryRepo.UpdateAsync(category);
        await _writeCategoryRepo.SaveChangeAsync();
        return HttpStatusCode.OK;
    }
}
















