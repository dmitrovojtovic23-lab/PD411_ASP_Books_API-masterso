using System.ComponentModel.DataAnnotations;

namespace PD411_Books.DAL.Entities
{
    public class RoleEntity : BaseEntity
    {
        [Required]
        public required string Name { get; set; }
        public List<UserEntity> Users { get; set; } = [];
    }
}
