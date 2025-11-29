using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Application.Interfaces;
using CommerceCleanArchitectureNET.Application.UseCases.Products;
using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.Tests.UseCases.Products
{
    public class CreateProductUseCaseTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CreateProductUseCase _useCase;

        public CreateProductUseCaseTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _useCase = new CreateProductUseCase(_mockRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidData_ShouldReturnSuccessResult()
        {
            // Arrange
            var dto = new CreateProductDto("Laptop", 999.99m, "USD", 10);

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product p, CancellationToken ct) => p);

            _mockUnitOfWork
                .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _useCase.ExecuteAsync(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(dto.Name, result.Value.Name);
            Assert.Equal(dto.Price, result.Value.Price);
            Assert.Equal(dto.Stock, result.Value.Stock);

            _mockRepository.Verify(
                r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidPrice_ShouldReturnFailureResult()
        {
            // Arrange
            var dto = new CreateProductDto("Laptop", -100, "USD", 10);

            // Act
            var result = await _useCase.ExecuteAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Contains("price", result.Error.ToLower());

            _mockRepository.Verify(
                r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task ExecuteAsync_WithInvalidName_ShouldReturnFailureResult(string invalidName)
        {
            // Arrange
            var dto = new CreateProductDto(invalidName, 100, "USD", 10);

            // Act
            var result = await _useCase.ExecuteAsync(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
        }
    }
}
