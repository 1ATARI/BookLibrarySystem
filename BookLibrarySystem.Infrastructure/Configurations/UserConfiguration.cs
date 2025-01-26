using BookLibrarySystem.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibrarySystem.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.OwnsOne(u => u.Name, name =>
        {
            name.Property(n => n.FirstName).HasColumnName("FirstName").IsRequired();
            name.Property(n => n.LastName).HasColumnName("LastName").IsRequired();
        });
        
        builder.HasMany(u => u.BorrowedBooks)
            .WithOne(ub => ub.ApplicationUser) // Assuming the UserBook entity has a navigation property for User.
            .HasForeignKey(ub => ub.UserId) // Foreign key for the UserBook relationship.
            .OnDelete(DeleteBehavior.Cascade); // Set delete behavior if needed.
    }
}