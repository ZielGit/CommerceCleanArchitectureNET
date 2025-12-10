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
    /// Especificación para buscar productos por nombre (contiene)
    /// </summary>
    public class ProductByNameSpecification : Specification<Product>
    {
        private readonly string _searchTerm;

        public ProductByNameSpecification(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new DomainException("Search term cannot be empty");

            _searchTerm = searchTerm.ToLowerInvariant();
        }

        public override bool IsSatisfiedBy(Product product)
        {
            return product.Name.ToLowerInvariant().Contains(_searchTerm);
        }
    }
}
