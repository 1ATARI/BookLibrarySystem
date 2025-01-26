using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Authors.UpdateAuthor;

public class UpdateAuthorCommandHandler : ICommandHandler<UpdateAuthorCommand, Author>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorRepository _authorRepository;

    public UpdateAuthorCommandHandler(IUnitOfWork unitOfWork, IAuthorRepository authorRepository)
    {
        _unitOfWork = unitOfWork;
        _authorRepository = authorRepository;
    }

    public async Task<Result<Author>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var author = await _authorRepository.GetByIdAsync(request.Id);

            if (author == null)
            {
                return Result.Failure<Author>(AuthorErrors.NotFound);
            }

            author.UpdateDetails(request.Name);  

            _authorRepository.Update(author);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(author);
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Author>(AuthorErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure<Author>(new Error("DatabaseError", ex.Message));
        }
    }
}
