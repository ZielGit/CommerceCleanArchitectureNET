using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Application.Interfaces;
using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Exceptions;
using CommerceCleanArchitectureNET.Domain.Repositories;
using CommerceCleanArchitectureNET.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products
{
    public class CreateProductUseCase : ICreateProductUseCase
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductUseCase(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductDto>> ExecuteAsync(CreateProductDto dto, CancellationToken ct = default)
        {
            try
            {
                var money = new Money(dto.Price, dto.Currency);
                var product = new Product(dto.Name, money, dto.Stock);

                await _repository.AddAsync(product, ct);
                await _unitOfWork.CommitAsync(ct);

                var productDto = MapToDto(product);
                return Result<ProductDto>.Success(productDto);
            }
            catch (DomainException ex)
            {
                return Result<ProductDto>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<ProductDto>.Failure($"Error creating product: {ex.Message}");
            }
        }

        private static ProductDto MapToDto(Product product) => new(
            product.Id,
            product.Name,
            product.Price.Amount,
            product.Price.Currency,
            product.Stock,
            product.IsActive
        );
    }
}
