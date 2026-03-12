using Microsoft.EntityFrameworkCore;
using PD411_Books.DAL.Entities;

namespace PD411_Books.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<BookEntity> Books { get; set; }
        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Books
            builder.Entity<BookEntity>(e =>
            {
                e.HasKey(b => b.Id);

                e.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

                e.Property(b => b.Description)
                .HasColumnType("text");

                e.Property(b => b.Image)
                .HasMaxLength(100);

                e.Property(b => b.Pages)
                .HasDefaultValue(0);

                e.Property(b => b.Rating)
                .HasDefaultValue(0f);
            });

            // Authors
            builder.Entity<AuthorEntity>(e =>
            {
                e.HasKey(a => a.Id);

                e.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(255);

                e.Property(a => a.Image)
                .HasMaxLength(100);

                e.Property(a => a.Country)
                .HasMaxLength(50);
            });

            // Genres
            builder.Entity<GenreEntity>(e =>
            {
                e.HasKey(g => g.Id);

                e.HasIndex(g => g.Name)
                .IsUnique();

                e.Property(g => g.Name)
                .HasMaxLength(50)
                .IsRequired();
            });

            // Relationships
            builder.Entity<BookEntity>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<BookEntity>()
                .HasMany(b => b.Genres)
                .WithMany(g => g.Books)
                .UsingEntity("BookGenres");

            base.OnModelCreating(builder);
        }
    }
}
