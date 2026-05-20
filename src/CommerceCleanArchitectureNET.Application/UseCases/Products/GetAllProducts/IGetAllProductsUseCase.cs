using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products.GetAllProducts
{
    public interface IGetAllProductsUseCase
    {
        Task<Result<PagedProductsDto>> ExecuteAsync(int page, int pageSize, CancellationToken ct = default);
    }
}
