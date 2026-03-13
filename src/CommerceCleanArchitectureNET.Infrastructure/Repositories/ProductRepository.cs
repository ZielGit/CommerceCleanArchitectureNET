using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Repositories;
using CommerceCleanArchitectureNET.Domain.Specifications;
using CommerceCleanArchitectureNET.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive)
                .ToListAsync(ct);
        }

        public async Task<Product> AddAsync(Product product, CancellationToken ct = default)
        {
            await _context.Products.AddAsync(product, ct);
            return product;
        }

        public Task UpdateAsync(Product product, CancellationToken ct = default)
        {
            _context.Products.Update(product);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var product = await _context.Products.FindAsync(new object[] { id }, ct);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Products.AnyAsync(p => p.Id == id, ct);
        }

        public async Task<IEnumerable<Product>> FindAsync(
            ISpecification<Product> specification,
            CancellationToken ct = default)
        {
            var allProducts = await _context.Products
                .AsNoTracking()
                .ToListAsync(ct);

            return allProducts.Where(p => specification.IsSatisfiedBy(p));
        }
    }
}
