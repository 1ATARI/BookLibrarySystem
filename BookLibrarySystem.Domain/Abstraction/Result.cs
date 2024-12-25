using System.Diagnostics.CodeAnalysis;

namespace BookLibrarySystem.Domain.Abstraction;

public class Result
{
    protected  Result(bool isSuccess, Error error)
    {
        //ensure result is always valid 
        // If isSuccess is true, the Error must be Error.None.
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        //If isSuccess is false, the Error cannot be Error.None
        //This prevents invalid combinations of success/failure states and errors
        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    //Returns a successful Result with no error.
    public static Result Success() => new(true, Error.None);

    //Returns a failure Result with the provided Error
    public static Result Failure(Error error) => new(false, error);

    // Creates a Result with a success state and a value
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    //Creates a Result with a failure state and an error.
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    //Returns Success if the provided value is not null.
    //Returns Failure with Error.NullValue if the value is null.
    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    //Stores the value if the result is successful.
    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /*
     * If the result is successful (IsSuccess), it returns the stored value.
     *If the result is a failure, accessing Value throws an exception to prevent misuse.
     */
    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}