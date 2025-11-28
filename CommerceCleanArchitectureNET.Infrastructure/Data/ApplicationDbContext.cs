using CommerceCleanArchitectureNET.Application.Interfaces;
using CommerceCleanArchitectureNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
        }

        public async Task<int> CommitAsync(CancellationToken ct = default)
        {
            return await SaveChangesAsync(ct);
        }

        public Task RollbackAsync(CancellationToken ct = default)
        {
            ChangeTracker.Clear();
            return Task.CompletedTask;
        }
    }
}
