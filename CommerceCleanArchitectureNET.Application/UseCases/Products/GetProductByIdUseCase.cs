using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products
{
    internal class GetProductByIdUseCase : IGetProductByIdUseCase
    {
        private readonly IProductRepository _repository;

        public GetProductByIdUseCase(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ProductDto>> ExecuteAsync(Guid id, CancellationToken ct = default)
        {
            var product = await _repository.GetByIdAsync(id, ct);

            if (product == null)
                return Result<ProductDto>.Failure("Product not found");

            var dto = new ProductDto(
                product.Id,
                product.Name,
                product.Price.Amount,
                product.Price.Currency,
                product.Stock,
                product.IsActive
            );

            return Result<ProductDto>.Success(dto);
        }
    }
}
