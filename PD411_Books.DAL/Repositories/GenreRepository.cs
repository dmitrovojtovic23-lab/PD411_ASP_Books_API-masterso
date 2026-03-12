using Microsoft.EntityFrameworkCore;
using PD411_Books.DAL.Entities;

namespace PD411_Books.DAL.Repositories
{
    public class GenreRepository : GenericRepository<GenreEntity>
    {
        public GenreRepository(AppDbContext context)
            : base(context)
        {
            
        }

        public IQueryable<GenreEntity> Genres => GetAll();

        public async Task<bool> IsExistAsync(string name)
        {
            return await Genres.AnyAsync(g => g.Name.ToLower() == name.ToLower());
        }

        public async Task<GenreEntity?> GetByNameAsync(string name)
        {
            return await Genres.FirstOrDefaultAsync(g => g.Name.ToLower() == name.ToLower());
        }
    }
}
