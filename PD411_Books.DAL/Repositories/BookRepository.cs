using Microsoft.EntityFrameworkCore;
using PD411_Books.DAL.Entities;

namespace PD411_Books.DAL.Repositories
{
    public class BookRepository : GenericRepository<BookEntity>
    {
        public BookRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<BookEntity> Books => GetAll();

        public async Task<List<BookEntity>> GetByGenreAsync(string genre)
        {
            return await Books
                .Include(b => b.Genres)
                .Where(b => b.Genres.Select(g => g.Name.ToLower()).Contains(genre.ToLower()))
                .ToListAsync();
        }
    }
}
