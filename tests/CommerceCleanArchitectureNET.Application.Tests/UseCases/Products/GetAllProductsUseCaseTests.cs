using CommerceCleanArchitectureNET.Application.UseCases.Products.GetAllProducts;
using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Repositories;
using CommerceCleanArchitectureNET.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.Tests.UseCases.Products
{
    public class GetAllProductsUseCaseTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetAllProductsUseCase _useCase;

        public GetAllProductsUseCaseTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _useCase = new GetAllProductsUseCase(_mockRepository.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
        {
            new Product("Laptop", new Money(999.99m, "USD"), 10),
            new Product("Mouse", new Money(29.99m, "USD"), 50),
            new Product("Keyboard", new Money(79.99m, "USD"), 25)
        };

            _mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await _useCase.ExecuteAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Count());

            var productsList = result.Value.ToList();
            Assert.Equal("Laptop", productsList[0].Name);
            Assert.Equal("Mouse", productsList[1].Name);
            Assert.Equal("Keyboard", productsList[2].Name);

            _mockRepository.Verify(
                r => r.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithNoProducts_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyList = new List<Product>();

            _mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyList);

            // Act
            var result = await _useCase.ExecuteAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task ExecuteAsync_WhenRepositoryThrowsException_ShouldReturnFailure()
        {
            // Arrange
            _mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _useCase.ExecuteAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Contains("Error retrieving products", result.Error);
        }
    }
}
