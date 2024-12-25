namespace BookLibrarySystem.Domain.UsersBooks;

public interface IUserBookRepository
{
    UserBook GetById(Guid userBookId);
    IEnumerable<UserBook> GetAll();
    IEnumerable<UserBook> GetByUserId(Guid userId);
    IEnumerable<UserBook> GetByBookId(Guid bookId);
    void Add(UserBook userBook);
    void Update(UserBook userBook);
    void Delete(Guid userBookId);
    bool Exists(Guid userBookId);
}