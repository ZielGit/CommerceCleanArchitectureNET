using CommerceCleanArchitectureNET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Domain.Specifications.ProductSpecifications
{
    /// <summary>
    /// Especificación para verificar si un producto tiene stock disponible
    /// </summary>
    public class ProductInStockSpecification : Specification<Product>
    {
        public override bool IsSatisfiedBy(Product product)
        {
            return product.Stock > 0;
        }
    }
}
