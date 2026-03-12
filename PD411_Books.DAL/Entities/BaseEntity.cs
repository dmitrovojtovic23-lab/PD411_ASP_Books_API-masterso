namespace PD411_Books.DAL.Entities
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime CreateDate { get; set; }
    }

    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
