using ECommerce.Application.Services;
using ECommerce.Domain.ViewModels;
using MediatR;

namespace ECommerce.Application.Behaviours.Queries.Product.GetAll;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
{
    private readonly IProductService _productService;

    public GetAllProductQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request,
        CancellationToken cancellationToken)
    {
        var allProductVm = await _productService.GetAllProductsAsync(new PaginationVM()
            { Page = request.Page, PageSize = request.PageSize });
        return new GetAllProductQueryResponse()
        {
            Products = allProductVm
        };
    }
}