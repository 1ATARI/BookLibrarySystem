using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.UpdateUserBook;


    

    internal sealed class UpdateUserBookCommandHandler : ICommandHandler<UpdateUserBookCommand, UserBook>
    {
        private readonly IUserBookRepository _userBookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserBookCommandHandler(IUserBookRepository userBookRepository, IUnitOfWork unitOfWork)
        {
            _userBookRepository = userBookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserBook>> Handle(UpdateUserBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userBook = await _userBookRepository.GetByIdAsync(request.UserBookId,cancellationToken);
                if (userBook == null)
                {
                    return Result.Failure<UserBook>(UserBookErrors.UserBookNotFound);
                }

                var updateBorrowedDateResult = userBook.UpdateBorrowedDate(request.BorrowedDate);
                if (updateBorrowedDateResult.IsFailure)
                {
                    return Result.Failure<UserBook>(updateBorrowedDateResult.Error);
                }

                if (request.ReturnedDate.HasValue)
                {
                    var updateReturnedDateResult = userBook.UpdateBorrowedDate(request.ReturnedDate.Value);
                    if (updateReturnedDateResult.IsFailure)
                    {
                        return Result.Failure<UserBook>(UserBookErrors.updateReturnedDateResult);
                    }
                }

                await _userBookRepository.UpdateAsync(userBook,cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success(userBook);
            }
            catch (ConcurrencyException)
            {
                return Result.Failure<UserBook>(UserBookErrors.Overlap);
            }
            catch (Exception ex)
            {
                return Result.Failure<UserBook>(new Error("DatabaseError", ex.Message));
            }
        }
    }



