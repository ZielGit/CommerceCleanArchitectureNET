using CommerceCleanArchitectureNET.Domain.Entities;
using CommerceCleanArchitectureNET.Domain.Repositories;
using CommerceCleanArchitectureNET.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CommerceCleanArchitectureNET.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, ct);
        }

        public async Task<User> AddAsync(User user, CancellationToken ct = default)
        {
            await _context.Users.AddAsync(user, ct);
            return user;
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _context.Users.AnyAsync(u => u.Email == email, ct);
        }
    }
}
