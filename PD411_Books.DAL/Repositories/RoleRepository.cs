using PD411_Books.DAL.Entities;

namespace PD411_Books.DAL.Repositories
{
    public class RoleRepository : GenericRepository<RoleEntity>
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<RoleEntity> Roles => GetAll();
    }
}
