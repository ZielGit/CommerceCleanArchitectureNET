using CommerceCleanArchitectureNET.Application.DTOs;
using CommerceCleanArchitectureNET.Application.Interfaces;
using CommerceCleanArchitectureNET.Application.UseCases.Products.UpdateProduct;
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
    public class UpdateProductUseCaseTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly UpdateProductUseCase _useCase;

        public UpdateProductUseCaseTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _useCase = new UpdateProductUseCase(_mockRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidData_ShouldUpdateProductStock()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("Laptop", new Money(999.99m, "USD"), 10);

            // Set Id using reflection
            var idProperty = typeof(Product).BaseType!
                .GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            idProperty!.SetValue(product, productId);

            var updateDto = new UpdateProductDto(Stock: 20);

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _useCase.ExecuteAsync(productId, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(20, result.Value.Stock);
            Assert.Equal(productId, result.Value.Id);

            _mockRepository.Verify(
                r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()),
                Times.Once);

            _mockRepository.Verify(
                r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithNonExistingProduct_ShouldReturnFailure()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var updateDto = new UpdateProductDto(Stock: 20);

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _useCase.ExecuteAsync(productId, updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Product not found", result.Error);

            _mockRepository.Verify(
                r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
                Times.Never);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithNegativeStock_ShouldReturnFailure()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("Laptop", new Money(999.99m, "USD"), 10);

            var idProperty = typeof(Product).BaseType!
                .GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            idProperty!.SetValue(product, productId);

            var updateDto = new UpdateProductDto(Stock: -5);

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _useCase.ExecuteAsync(productId, updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("stock cannot be negative", result.Error.ToLower());

            _mockRepository.Verify(
                r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
