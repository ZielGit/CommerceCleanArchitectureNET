using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Application.Interfaces;
using CommerceCleanArchitectureNET.Domain.Exceptions;
using CommerceCleanArchitectureNET.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products.UpdateProduct
{
    public class UpdateProductUseCase : IUpdateProductUseCase
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductUseCase(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductDto>> ExecuteAsync(
            Guid id,
            UpdateProductDto dto,
            CancellationToken ct = default)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id, ct);

                if (product == null)
                    return Result<ProductDto>.Failure("Product not found");

                // Actualizar el stock
                product.UpdateStock(dto.Stock);

                await _repository.UpdateAsync(product, ct);
                await _unitOfWork.CommitAsync(ct);

                var productDto = new ProductDto(
                    product.Id,
                    product.Name,
                    product.Price.Amount,
                    product.Price.Currency,
                    product.Stock,
                    product.IsActive
                );

                return Result<ProductDto>.Success(productDto);
            }
            catch (DomainException ex)
            {
                return Result<ProductDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<ProductDto>.Failure($"Error updating product: {ex.Message}");
            }
        }
    }
}
