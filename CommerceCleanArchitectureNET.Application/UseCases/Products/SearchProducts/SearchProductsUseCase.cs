using CommerceCleanArchitectureNET.Application.Common;
using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Exceptions;
using CommerceCleanArchitectureNET.Domain.Repositories;
using CommerceCleanArchitectureNET.Domain.Specifications;
using CommerceCleanArchitectureNET.Domain.Specifications.ProductSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.UseCases.Products.SearchProducts
{
    /// <summary>
    /// Caso de uso para buscar productos usando el patrón Specification
    /// </summary>
    public class SearchProductsUseCase : ISearchProductsUseCase
    {
        private readonly IProductRepository _repository;

        public SearchProductsUseCase(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<ProductDto>>> ExecuteAsync(
            ProductSearchDto searchDto,
            CancellationToken ct = default)
        {
            try
            {
                // Construir la especificación compuesta
                ISpecification<Product>? specification = null;

                // Filtro por nombre
                if (!string.IsNullOrWhiteSpace(searchDto.Name))
                {
                    var nameSpec = new ProductByNameSpecification(searchDto.Name);
                    specification = specification == null ? nameSpec : specification.And(nameSpec);
                }

                // Filtro por rango de precios
                if (searchDto.MinPrice.HasValue || searchDto.MaxPrice.HasValue)
                {
                    var minPrice = searchDto.MinPrice ?? 0;
                    var maxPrice = searchDto.MaxPrice ?? decimal.MaxValue;
                    var priceSpec = new ProductByPriceRangeSpecification(minPrice, maxPrice);
                    specification = specification == null ? priceSpec : specification.And(priceSpec);
                }

                // Filtro por stock
                if (searchDto.OnlyInStock == true)
                {
                    var stockSpec = new ProductInStockSpecification();
                    specification = specification == null ? stockSpec : specification.And(stockSpec);
                }

                // Filtro por productos activos
                if (searchDto.OnlyActive == true)
                {
                    var activeSpec = new ActiveProductsSpecification();
                    specification = specification == null ? activeSpec : specification.And(activeSpec);
                }

                // Si no hay filtros, obtener todos
                var products = specification == null
                    ? await _repository.GetAllAsync(ct)
                    : await _repository.FindAsync(specification, ct);

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
            catch (DomainException ex)
            {
                return Result<IEnumerable<ProductDto>>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ProductDto>>.Failure(
                    $"Error searching products: {ex.Message}");
            }
        }
    }
}
