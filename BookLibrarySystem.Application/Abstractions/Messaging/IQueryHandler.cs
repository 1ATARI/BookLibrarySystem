using BookLibrarySystem.Domain.Abstraction;
using MediatR;

namespace BookLibrarySystem.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IRequest<Result<TResponse>>
{
    
}