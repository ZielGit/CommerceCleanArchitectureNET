using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Domain.Repositories;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products.GetAllProducts
{
    public class GetAllProductsUseCase : IGetAllProductsUseCase
    {
        private readonly IProductRepository _repository;

        public GetAllProductsUseCase(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PagedProductsDto>> ExecuteAsync(int page, int pageSize, CancellationToken ct = default)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100;

                var (items, total) = await _repository.GetPagedAsync(page, pageSize, ct);

                var products = items.Select(p => new ProductDto(
                    p.Id,
                    p.Name,
                    p.Price.Amount,
                    p.Price.Currency,
                    p.Stock,
                    p.IsActive
                ));

                var totalPages = (int)Math.Ceiling(total / (double)pageSize);

                return Result<PagedProductsDto>.Success(new PagedProductsDto(products, total, page, pageSize, totalPages));
            }
            catch (Exception ex)
            {
                return Result<PagedProductsDto>.Failure($"Error retrieving products: {ex.Message}");
            }
        }
    }
}
