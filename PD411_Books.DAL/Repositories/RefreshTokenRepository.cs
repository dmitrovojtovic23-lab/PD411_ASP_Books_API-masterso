using Microsoft.EntityFrameworkCore;
using PD411_Books.DAL.Entities;

namespace PD411_Books.DAL.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshTokenEntity>
    {
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<RefreshTokenEntity> RefreshTokens => GetAll();

        public async Task<List<RefreshTokenEntity>> GetExpiredTokensAsync()
        {
            return await RefreshTokens
                .Where(rt => rt.Expires < DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<int> DeleteExpiredTokensAsync()
        {
            var expiredTokens = await GetExpiredTokensAsync();
            _context.RefreshTokens.RemoveRange(expiredTokens);
            return await _context.SaveChangesAsync();
        }
    }
}
