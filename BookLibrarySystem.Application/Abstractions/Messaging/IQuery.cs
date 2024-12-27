using BookLibrarySystem.Domain.Abstraction;
using MediatR;

namespace BookLibrarySystem.Application.Abstractions.Messaging;
//TResponse for the  return type of this query 
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}