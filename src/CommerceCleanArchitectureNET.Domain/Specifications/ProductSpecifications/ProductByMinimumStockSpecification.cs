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
    /// Especificación para verificar si un producto tiene al menos una cantidad mínima de stock
    /// </summary>
    public class ProductByMinimumStockSpecification : Specification<Product>
    {
        private readonly int _minimumStock;

        public ProductByMinimumStockSpecification(int minimumStock)
        {
            if (minimumStock < 0)
                throw new DomainException("Minimum stock cannot be negative");

            _minimumStock = minimumStock;
        }

        public override bool IsSatisfiedBy(Product product)
        {
            return product.Stock >= _minimumStock;
        }
    }
}
