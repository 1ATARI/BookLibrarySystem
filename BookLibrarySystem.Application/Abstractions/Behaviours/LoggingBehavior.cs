using BookLibrarySystem.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookLibrarySystem.Application.Abstractions.Behaviours;
/// <summary>
/// Pipeline behavior that logs the execution of commands in the MediatR pipeline.
/// This behavior logs the start of command execution, successful completion, and any failures.
/// It provides useful logging for debugging and monitoring the flow of commands through the system.
/// This is applied to commands that implement the <see cref="IBaseCommand"/> interface.
/// </summary>
/// <typeparam name="TRequest">
/// The type of the request (command) being processed, which must implement the <see cref="IBaseCommand"/> interface.
/// </typeparam>
/// <typeparam name="TResponse">
/// The type of the response that is returned after the command has been handled. This is the result of the command execution.
/// </typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation("Executing command {Command}", name);

            var result = await next();

            _logger.LogInformation("Command {Command} processed successfully", name);

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Command {Command} processing failed", name);

            throw;
        }
    }
}
