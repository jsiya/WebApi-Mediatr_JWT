using System.Net;
using ECommerce.Application.Repositories;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;

namespace ECommerce.Persistence.Services;

public class ProductService : IProductService
{
    private readonly IReadProductRepository _readProductRepo;
    private readonly IWriteProductRepository _writeProductRepo;
    private readonly IReadCategoryRepository _readCategoryRepository;
    


    public ProductService(IReadProductRepository readProductRepo, IWriteProductRepository writeProductRepo, IReadCategoryRepository readCategoryRepository)
    {
        _readProductRepo = readProductRepo;
        _writeProductRepo = writeProductRepo;
        _readCategoryRepository = readCategoryRepository;
    }

    public async Task<ICollection<AllProductVM>> GetAllProductsAsync(PaginationVM paginationVM)
    {
        var products = await _readProductRepo.GetAllAsync();
        var prodcutForPage = products.ToList().
            Skip(paginationVM.Page*paginationVM.PageSize).Take(paginationVM.PageSize).ToList();


        var allProductVm = prodcutForPage.Select(p => new AllProductVM()
        {
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            CategoryName = p.Category.Name,
            ImageUrl = p.ImageUrl,
            Stock = p.Stock
        }).ToList();
        return allProductVm;
    }

    public Task<AllProductVM?> GetProductByIdAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public async Task AddProductAsync(AddProductVM productVM)
    {
        
        var product = new Product()
        {
            Name = productVM.Name,
            Price = productVM.Price,
            Description = productVM.Description,
            CategoryId = productVM.CategoryId,
            Stock = productVM.Stock,
        };

        await _writeProductRepo.AddAsync(product);
        await _writeProductRepo.SaveChangeAsync();
    }

    public async Task<HttpStatusCode> UpdateProductAsync(int id, AddProductVM updateProductVM)
    {
        var product = await _readProductRepo.GetByIdAsync(id);
        if (product is null)
            return HttpStatusCode.NotFound;
        
        var category = await _readCategoryRepository.GetByIdAsync(updateProductVM.CategoryId);
        if (category == null)
            return HttpStatusCode.NotFound;
        
        product.Name = updateProductVM.Name;
        product.Description = updateProductVM.Description;
        product.Stock = updateProductVM.Stock;
        product.Price = updateProductVM.Price;
        product.CategoryId = updateProductVM.CategoryId;
        await _writeProductRepo.UpdateAsync(product);
        await _writeProductRepo.SaveChangeAsync();
        return HttpStatusCode.OK;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var product = await _readProductRepo.GetByIdAsync(productId);
        if (product is null)
            return false;
        await _writeProductRepo.DeleteAsync(productId);
        await _writeProductRepo.SaveChangeAsync();
        return true;
    }
}