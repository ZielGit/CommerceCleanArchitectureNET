using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Domain.Specifications.ProductSpecifications
{
    /// <summary>
    /// Especificación para verificar si un producto está dentro de un rango de precios
    /// </summary>
    public class ProductByPriceRangeSpecification : Specification<Product>
    {
        private readonly decimal _minPrice;
        private readonly decimal _maxPrice;

        public ProductByPriceRangeSpecification(decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0)
                throw new DomainException("Minimum price cannot be negative");

            if (maxPrice < minPrice)
                throw new DomainException("Maximum price cannot be less than minimum price");

            _minPrice = minPrice;
            _maxPrice = maxPrice;
        }

        public override bool IsSatisfiedBy(Product product)
        {
            return product.Price.Amount >= _minPrice && product.Price.Amount <= _maxPrice;
        }
    }
}
