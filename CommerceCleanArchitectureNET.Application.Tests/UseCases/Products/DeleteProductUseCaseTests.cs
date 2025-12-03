using CommerceCleanArchitectureNET.Application.Interfaces;
using CommerceCleanArchitectureNET.Application.UseCases.Products.DeleteProduct;
using CommerceCleanArchitectureNET.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Application.Tests.UseCases.Products
{
    public class DeleteProductUseCaseTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly DeleteProductUseCase _useCase;

        public DeleteProductUseCaseTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _useCase = new DeleteProductUseCase(_mockRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithExistingProduct_ShouldDeleteSuccessfully()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _mockRepository
                .Setup(r => r.ExistsAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mockRepository
                .Setup(r => r.DeleteAsync(productId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _useCase.ExecuteAsync(productId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);

            _mockRepository.Verify(
                r => r.ExistsAsync(productId, It.IsAny<CancellationToken>()),
                Times.Once);

            _mockRepository.Verify(
                r => r.DeleteAsync(productId, It.IsAny<CancellationToken>()),
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

            _mockRepository
                .Setup(r => r.ExistsAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _useCase.ExecuteAsync(productId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Product not found", result.Error);

            _mockRepository.Verify(
                r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
                Times.Never);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenDeleteFails_ShouldReturnFailure()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _mockRepository
                .Setup(r => r.ExistsAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mockRepository
                .Setup(r => r.DeleteAsync(productId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database connection error"));

            // Act
            var result = await _useCase.ExecuteAsync(productId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Contains("Error deleting product", result.Error);
        }
    }
}
