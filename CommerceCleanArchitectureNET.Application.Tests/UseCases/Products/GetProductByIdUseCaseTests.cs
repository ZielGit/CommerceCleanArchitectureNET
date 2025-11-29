using CommerceCleanArchitectureNET.Application.UseCases.Products;
using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Repositories;
using CommerceCleanArchitectureNET.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.Tests.UseCases.Products
{
    public class GetProductByIdUseCaseTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetProductByIdUseCase _useCase;

        public GetProductByIdUseCaseTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _useCase = new GetProductByIdUseCase(_mockRepository.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithExistingProduct_ShouldReturnSuccessResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("Laptop", new Money(999.99m, "USD"), 10);

            // Set Id using reflection (for testing purposes)
            var idProperty = typeof(Product).BaseType!
                .GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            idProperty!.SetValue(product, productId);

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _useCase.ExecuteAsync(productId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(productId, result.Value.Id);
            Assert.Equal("Laptop", result.Value.Name);

            _mockRepository.Verify(
                r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithNonExistingProduct_ShouldReturnFailureResult()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _useCase.ExecuteAsync(productId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Product not found", result.Error);

            _mockRepository.Verify(
                r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
