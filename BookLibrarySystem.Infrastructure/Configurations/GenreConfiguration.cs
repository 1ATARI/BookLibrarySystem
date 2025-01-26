using BookLibrarySystem.Domain.Genres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibrarySystem.Infrastructure.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.OwnsOne(g => g.Name, name =>
        {
            name.Property(n => n.Value).HasColumnName("Name").IsRequired();
        });        builder.OwnsOne(g => g.Description, description =>
        {
            description.Property(d => d.Value).HasColumnName("Description").IsRequired();
        });        
        
        
        builder.HasMany(g => g.Books)
            .WithOne(bg => bg.Genre)
            .HasForeignKey(bg => bg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
 
    }
}