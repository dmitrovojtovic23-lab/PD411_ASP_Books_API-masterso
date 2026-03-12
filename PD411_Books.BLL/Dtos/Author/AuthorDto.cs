namespace PD411_Books.BLL.Dtos.Author
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public string? Image { get; set; }
        public string? Country { get; set; }
    }
}
