using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Exceptions;
using CommerceCleanArchitectureNET.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Domain.Tests.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateProduct()
        {
            // Arrange
            var name = "Laptop";
            var price = new Money(999.99m, "USD");
            var stock = 10;

            // Act
            var product = new Product(name, price, stock);

            // Assert
            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.Equal(name, product.Name);
            Assert.Equal(price, product.Price);
            Assert.Equal(stock, product.Stock);
            Assert.True(product.IsActive);
            Assert.True(product.CreatedAt <= DateTime.UtcNow);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithInvalidName_ShouldThrowDomainException(string invalidName)
        {
            // Arrange
            var price = new Money(100, "USD");
            var stock = 5;

            // Act & Assert
            var exception = Assert.Throws<DomainException>(
                () => new Product(invalidName, price, stock));

            Assert.Equal("Product name cannot be empty", exception.Message);
        }

        [Fact]
        public void Constructor_WithNegativeStock_ShouldThrowDomainException()
        {
            // Arrange
            var name = "Laptop";
            var price = new Money(100, "USD");
            var stock = -5;

            // Act & Assert
            var exception = Assert.Throws<DomainException>(
                () => new Product(name, price, stock));

            Assert.Equal("Product stock cannot be negative", exception.Message);
        }

        [Fact]
        public void UpdateStock_WithValidQuantity_ShouldUpdateStock()
        {
            // Arrange
            var product = new Product("Laptop", new Money(100, "USD"), 10);
            var newStock = 20;

            // Act
            product.UpdateStock(newStock);

            // Assert
            Assert.Equal(newStock, product.Stock);
            Assert.NotNull(product.UpdatedAt);
        }

        [Fact]
        public void Deactivate_ShouldSetIsActiveToFalse()
        {
            // Arrange
            var product = new Product("Laptop", new Money(100, "USD"), 10);

            // Act
            product.Deactivate();

            // Assert
            Assert.False(product.IsActive);
            Assert.NotNull(product.UpdatedAt);
        }
    }
}
