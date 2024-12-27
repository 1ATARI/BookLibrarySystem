using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace BookLibrarySystem.Application.Abstractions.Behaviours;
/// <summary>
/// Pipeline behavior that validates commands (requests) in the MediatR pipeline using FluentValidation.
/// This behavior ensures that commands are validated before being processed by their handlers.
/// If validation fails, it throws a <see cref="ValidationException"/> containing the validation errors.
/// </summary>
/// <typeparam name="TRequest">
/// The type of the request (command) being validated. It must implement the <see cref="IBaseCommand"/> interface.
/// </typeparam>
/// <typeparam name="TResponse">
/// The type of the response returned after the command has been processed successfully.
/// </typeparam>

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        //validate the request obect
        if (!_validators.Any())
        {
            return await next();
        }
            
        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
            .Select(validator => validator.Validate(context))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage))
            .Distinct()
            .ToList();

        if (validationErrors.Any())
        {
            throw new Exceptions.ValidationException(validationErrors);
        }

        return await next();
    }
}