namespace Facturity.Modules.SharedKernel.Domain;

public interface IResultBase
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    IReadOnlyList<Error> Errors { get; }
    Error FirstError { get; }
}

public readonly record struct Result : IResultBase
{
    private readonly List<Error> _errors;

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<Error> Errors => _errors.AsReadOnly();

    public Error FirstError => _errors.Count > 0
        ? _errors[0]
        : Error.None;

    private Result(bool isSuccess, List<Error> errors)
    {
        IsSuccess = isSuccess;
        _errors = errors;
    }

    public static Result Ok() =>
        new(true, []);

    public static Result Fail(Error error) =>
        new(false, [error]);

    public static Result Fail(IEnumerable<Error> errors)
    {
        var list = errors.ToList();

        if (list.Count == 0)
            throw new InvalidOperationException(
                "At least one error must be provided.");

        return new(false, list);
    }

    public static Result<TValue> Ok<TValue>(TValue value) =>
        Result<TValue>.Ok(value);

    public static Result<TValue> Fail<TValue>(Error error) =>
        Result<TValue>.Fail(error);

    public static Result<TValue> Fail<TValue>(IEnumerable<Error> errors) =>
        Result<TValue>.Fail(errors);
}

public readonly record struct Result<TValue> : IResultBase
{
    private readonly List<Error> _errors;
    private readonly TValue? _value;

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<Error> Errors => _errors.AsReadOnly();

    public Error FirstError => _errors.Count > 0
        ? _errors[0]
        : Error.None;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException(
            "Cannot access the value of a failed result.");

    private Result(TValue? value, bool isSuccess, List<Error> errors)
    {
        _value = value;
        IsSuccess = isSuccess;
        _errors = errors;
    }

    public static Result<TValue> Ok(TValue value) =>
        new(value, true, []);

    public static Result<TValue> Fail(Error error) =>
        new(default, false, [error]);

    public static Result<TValue> Fail(IEnumerable<Error> errors)
    {
        var list = errors.ToList();

        if (list.Count == 0)
            throw new InvalidOperationException(
                "At least one error must be provided.");

        return new(default, false, list);
    }

    public Result<TNext> Bind<TNext>(Func<TValue, Result<TNext>> func) =>
        IsSuccess ? func(_value!) : Result<TNext>.Fail(_errors);

    public Result<TNext> Map<TNext>(Func<TValue, TNext> func) =>
        IsSuccess ? Result<TNext>.Ok(func(_value!)) : Result<TNext>.Fail(_errors);

    public TNext Match<TNext>(
        Func<TValue, TNext> onSuccess,
        Func<IReadOnlyList<Error>, TNext> onFailure) =>
        IsSuccess ? onSuccess(_value!) : onFailure(_errors);

    public static implicit operator Result<TValue>(TValue value) =>
        Ok(value);

    public static implicit operator Result<TValue>(Error error) =>
        Fail(error);
}
