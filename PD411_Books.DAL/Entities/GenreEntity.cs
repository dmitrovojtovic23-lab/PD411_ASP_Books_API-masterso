namespace PD411_Books.DAL.Entities
{
    public class GenreEntity : BaseEntity
    {
        public required string Name { get; set; }

        public List<BookEntity> Books { get; set; } = [];
    }
}
