using BookLibrarySystem.Domain.UsersBooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibrarySystem.Infrastructure.Configurations;

public class UserBookConfiguration : IEntityTypeConfiguration<UserBook>
{
    public void Configure(EntityTypeBuilder<UserBook> builder)
    {
        builder.HasOne(ub => ub.ApplicationUser)
            .WithMany(u => u.BorrowedBooks)
            .HasForeignKey(ub => ub.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ub => ub.Book)
            .WithMany() 
            .HasForeignKey(ub => ub.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ub => ub.BorrowedDate).IsRequired();
        builder.Property(ub => ub.ReturnedDate).IsRequired(false);
    }
}