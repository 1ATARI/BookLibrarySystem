using BookLibrarySystem.Domain.Authors;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.BooksGenres;
using BookLibrarySystem.Domain.Genres;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibrarySystem.Infrastructure.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(book => book.Id);
        builder.OwnsOne(book => book.Title, description =>
        {
            description.Property(d => d.Value).HasColumnName("Title").IsRequired();
        });
        builder.OwnsOne(book => book.Description, description =>
        {
            description.Property(d => d.Value).HasColumnName("Description").IsRequired();
        });
        builder.Property(book => book.PublicationDate)
            .IsRequired();
        builder.Property(book => book.Pages)
            .IsRequired()
            .HasDefaultValue(0);
        builder.Property(book => book.IsAvailable)
            .IsRequired()
            .HasDefaultValue(true);

        
        
        builder.HasMany(b => b.Genres)
            .WithOne(bg => bg.Book)
            .HasForeignKey(bg => bg.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(book => book.Author)
            .WithMany(author => author.Books)
            .HasForeignKey(book => book.AuthorId);
        
    }
}