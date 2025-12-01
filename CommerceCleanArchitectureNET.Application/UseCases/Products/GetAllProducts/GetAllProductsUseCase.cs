using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products.GetAllProducts
{
    public class GetAllProductsUseCase : IGetAllProductsUseCase
    {
        private readonly IProductRepository _repository;

        public GetAllProductsUseCase(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<ProductDto>>> ExecuteAsync(CancellationToken ct = default)
        {
            try
            {
                var products = await _repository.GetAllAsync(ct);

                var productDtos = products.Select(p => new ProductDto(
                    p.Id,
                    p.Name,
                    p.Price.Amount,
                    p.Price.Currency,
                    p.Stock,
                    p.IsActive
                ));

                return Result<IEnumerable<ProductDto>>.Success(productDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ProductDto>>.Failure($"Error retrieving products: {ex.Message}");
            }
        }
    }
}
