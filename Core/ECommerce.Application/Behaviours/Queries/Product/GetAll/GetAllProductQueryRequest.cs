using MediatR;

namespace ECommerce.Application.Behaviours.Queries.Product.GetAll;

public class GetAllProductQueryRequest: IRequest<GetAllProductQueryResponse>
{
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}