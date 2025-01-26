using BookLibrarySystem.Domain.Authors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibrarySystem.Infrastructure.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.OwnsOne(a => a.Name, name =>
        {
            name.Property(n => n.FirstName).HasColumnName("FirstName").IsRequired();
            name.Property(n => n.LastName).HasColumnName("LastName").IsRequired();
        });
        
        builder.HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId) 
            .OnDelete(DeleteBehavior.Cascade);
    }
}