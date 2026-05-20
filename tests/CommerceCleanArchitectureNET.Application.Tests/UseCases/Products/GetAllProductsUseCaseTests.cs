using CommerceCleanArchitectureNET.Application.UseCases.Products.GetAllProducts;
using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Repositories;
using CommerceCleanArchitectureNET.Domain.ValueObjects;
using Moq;

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
        public async Task ExecuteAsync_WithProducts_ShouldReturnPagedProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product("Laptop", new Money(999.99m, "USD"), 10),
                new Product("Mouse", new Money(29.99m, "USD"), 50),
                new Product("Keyboard", new Money(79.99m, "USD"), 25)
            };

            _mockRepository
                .Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync((products, products.Count));

            // Act
            var result = await _useCase.ExecuteAsync(1, 10);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Data.Count());
            Assert.Equal(1, result.Value.Page);
            Assert.Equal(10, result.Value.PageSize);
            Assert.Equal(3, result.Value.Total);
            Assert.Equal(1, result.Value.TotalPages);

            _mockRepository.Verify(
                r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithNoProducts_ShouldReturnEmptyPagedResult()
        {
            // Arrange
            _mockRepository
                .Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync((new List<Product>(), 0));

            // Act
            var result = await _useCase.ExecuteAsync(1, 10);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Data);
            Assert.Equal(0, result.Value.Total);
            Assert.Equal(0, result.Value.TotalPages);
        }

        [Fact]
        public async Task ExecuteAsync_WhenRepositoryThrowsException_ShouldReturnFailure()
        {
            // Arrange
            _mockRepository
                .Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _useCase.ExecuteAsync(1, 10);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Contains("Error retrieving products", result.Error);
        }

        [Fact]
        public async Task ExecuteAsync_WithSecondPage_ShouldPassCorrectPaginationParams()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product("Tablet", new Money(499.99m, "USD"), 15)
            };

            _mockRepository
                .Setup(r => r.GetPagedAsync(2, 5, It.IsAny<CancellationToken>()))
                .ReturnsAsync((products, 6));

            // Act
            var result = await _useCase.ExecuteAsync(2, 5);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value!.Page);
            Assert.Equal(5, result.Value.PageSize);
            Assert.Equal(6, result.Value.Total);
            Assert.Equal(2, result.Value.TotalPages);
        }
    }
}
