using PD411_Books.DAL.Entities;

namespace PD411_Books.DAL.Repositories
{
    public class UserRepository : GenericRepository<UserEntity>
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<UserEntity> Users => GetAll();
    }
}
