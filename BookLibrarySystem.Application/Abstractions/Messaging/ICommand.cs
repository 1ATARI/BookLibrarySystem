using BookLibrarySystem.Domain.Abstraction;
using MediatR;

namespace BookLibrarySystem.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>,IBaseCommand
{
}
public interface ICommand<TResponse> : IRequest<Result<TResponse>>,IBaseCommand
{
}
public interface IBaseCommand 
{
}