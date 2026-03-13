using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Specifications.ProductSpecifications;
using CommerceCleanArchitectureNET.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Domain.Tests.Specifications
{
    public class ProductSpecificationTests
    {
        [Fact]
        public void ActiveProductsSpecification_WithActiveProduct_ShouldReturnTrue()
        {
            // Arrange
            var product = new Product("Laptop", new Money(999.99m, "USD"), 10);
            var spec = new ActiveProductsSpecification();

            // Act
            var result = spec.IsSatisfiedBy(product);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ProductInStockSpecification_WithStockGreaterThanZero_ShouldReturnTrue()
        {
            // Arrange
            var product = new Product("Mouse", new Money(29.99m, "USD"), 5);
            var spec = new ProductInStockSpecification();

            // Act
            var result = spec.IsSatisfiedBy(product);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ProductByPriceRangeSpecification_WithPriceInRange_ShouldReturnTrue()
        {
            // Arrange
            var product = new Product("Keyboard", new Money(79.99m, "USD"), 15);
            var spec = new ProductByPriceRangeSpecification(50m, 100m);

            // Act
            var result = spec.IsSatisfiedBy(product);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CombinedSpecifications_UsingAnd_ShouldWorkCorrectly()
        {
            // Arrange
            var product = new Product("Monitor", new Money(299.99m, "USD"), 8);
            var activeSpec = new ActiveProductsSpecification();
            var stockSpec = new ProductInStockSpecification();
            var combinedSpec = activeSpec.And(stockSpec);

            // Act
            var result = combinedSpec.IsSatisfiedBy(product);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CombinedSpecifications_UsingOr_ShouldWorkCorrectly()
        {
            // Arrange
            var product = new Product("Headphones", new Money(149.99m, "USD"), 0);
            product.Deactivate();

            var activeSpec = new ActiveProductsSpecification();
            var stockSpec = new ProductInStockSpecification();
            var combinedSpec = activeSpec.Or(stockSpec);

            // Act
            var result = combinedSpec.IsSatisfiedBy(product);

            // Assert
            Assert.False(result); // Ni activo ni en stock
        }

        [Fact]
        public void NotSpecification_ShouldNegateResult()
        {
            // Arrange
            var product = new Product("Webcam", new Money(89.99m, "USD"), 0);
            var stockSpec = new ProductInStockSpecification();
            var notStockSpec = stockSpec.Not();

            // Act
            var result = notStockSpec.IsSatisfiedBy(product);

            // Assert
            Assert.True(result); // No tiene stock, por lo tanto NOT es true
        }
    }
}
