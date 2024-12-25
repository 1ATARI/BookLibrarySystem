namespace BookLibrarySystem.Domain.Users;

public interface IUserRepository
{
    User GetById(Guid userId);
    IEnumerable<User> GetAll();
    void Add(User user);
    void Update(User user);
    void Delete(Guid userId);
    bool Exists(Guid userId);
}