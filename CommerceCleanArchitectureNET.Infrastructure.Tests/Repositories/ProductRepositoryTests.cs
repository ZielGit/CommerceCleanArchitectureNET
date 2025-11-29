using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.ValueObjects;
using CommerceCleanArchitectureNET.Infrastructure.Data;
using CommerceCleanArchitectureNET.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Infrastructure.Tests.Repositories
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ProductRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProductToDatabase()
        {
            // Arrange
            var product = new Product("Laptop", new Money(999.99m, "USD"), 10);

            // Act
            await _repository.AddAsync(product);
            await _context.SaveChangesAsync();

            // Assert
            var savedProduct = await _context.Products.FindAsync(product.Id);
            Assert.NotNull(savedProduct);
            Assert.Equal(product.Name, savedProduct.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingProduct_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product("Laptop", new Money(999.99m, "USD"), 10);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistingProduct_ShouldReturnNull()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(nonExistingId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnlyActiveProducts()
        {
            // Arrange
            var activeProduct = new Product("Laptop", new Money(999.99m, "USD"), 10);
            var inactiveProduct = new Product("Mouse", new Money(29.99m, "USD"), 5);
            inactiveProduct.Deactivate();

            await _context.Products.AddRangeAsync(activeProduct, inactiveProduct);
            await _context.SaveChangesAsync();

            // Act
            var results = await _repository.GetAllAsync();

            // Assert
            Assert.Single(results);
            Assert.Contains(results, p => p.Id == activeProduct.Id);
            Assert.DoesNotContain(results, p => p.Id == inactiveProduct.Id);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
