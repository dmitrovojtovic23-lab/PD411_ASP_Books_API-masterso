using Microsoft.EntityFrameworkCore;
using PD411_Books.DAL.Entities;

namespace PD411_Books.DAL.Repositories
{
    public class AuthorRepository : GenericRepository<AuthorEntity>
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<AuthorEntity> Authors => GetAll();

        public async Task<AuthorEntity?> GetByNameAsync(string name)
        {
            return await Authors
                .FirstOrDefaultAsync(a => a.Name.ToLower() == name.ToLower());
        }

        public async Task<List<BookEntity>> GetBooksAsync(AuthorEntity entity)
        {
            return await _context.Books
                .AsNoTracking()
                .Where(b => b.AuthorId == entity.Id)
                .ToListAsync();
        }

        public async Task<bool> AddBookAsync(AuthorEntity author, BookEntity book)
        {
            var authorBooks = await GetBooksAsync(author);

            if(!authorBooks.Contains(book))
            {
                author.Books.Add(book);
                return (await _context.SaveChangesAsync()) != 0;
            }

            return false;
        }
    }
}
