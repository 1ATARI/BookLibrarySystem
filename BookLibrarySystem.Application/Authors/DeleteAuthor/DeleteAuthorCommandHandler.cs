using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;

namespace BookLibrarySystem.Application.Authors.DeleteAuthor;

public class DeleteAuthorCommandHandler : ICommandHandler<DeleteAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAuthorCommandHandler(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
    {
        _authorRepository = authorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var author = await _authorRepository.GetByIdAsync(request.AuthorId, "Books", cancellationToken);

            if (author == null)
            {
                return Result.Failure(AuthorErrors.NotFound);
            }

            if (author.Books.Any())
            {
                return Result.Failure(AuthorErrors.CannotDeleteAuthorWithBooks);
            }

            var isDeleted = await _authorRepository.DeleteAsync(request.AuthorId ,cancellationToken);

            if (!isDeleted)
            {
                return Result.Failure(AuthorErrors.DeleteFailed);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (ConcurrencyException)
        {
            return Result.Failure(AuthorErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("DatabaseError", ex.Message));
        }
    }
}