using BookLibrarySystem.Application.Abstractions.Email;
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace BookLibrarySystem.Application.Users.DeleteUser;

internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Result>
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public DeleteUserCommandHandler(IApplicationUserRepository applicationUserRepository, UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _applicationUserRepository = applicationUserRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<Result<Result>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _applicationUserRepository.GetByIdAsync(request.UserId,null, cancellationToken);
            if (user == null)
            {
                return Result.Failure(ApplicationUserErrors.NotFound);
            }

            var identityResult = await _userManager.DeleteAsync(user);
            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                return Result.Failure(new Error("UserDeletionFailed", errors));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var subject = "Your Account Has Been Deleted";
            var message = $"Hello {user.Name.FirstName},\n\n" +
                          "We're sorry to see you go. Your account has been successfully deleted.\n" +
                          "If this was a mistake or if you have any feedback for us, please let us know.\n\n" +
                          "Best regards,\n" +
                          "Book Library System Team";

            // Send deletion confirmation email
            await _emailService.SendAsync(user.Email, subject, message);
            return Result.Success();
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<ApplicationUser>(ApplicationUserErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("DatabaseError", ex.Message));
        }
    }
}