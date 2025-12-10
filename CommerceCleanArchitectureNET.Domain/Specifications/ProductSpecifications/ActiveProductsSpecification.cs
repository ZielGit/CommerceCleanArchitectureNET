using CommerceCleanArchitectureNET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Domain.Specifications.ProductSpecifications
{
    /// <summary>
    /// Especificación para verificar si un producto está activo
    /// </summary>
    public class ActiveProductsSpecification : Specification<Product>
    {
        public override bool IsSatisfiedBy(Product product)
        {
            return product.IsActive;
        }
    }
}
