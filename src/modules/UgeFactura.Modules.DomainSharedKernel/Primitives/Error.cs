namespace UgeFactura.Modules.DomainSharedKernel;

public enum ErrorType
{
    None,
    Validation,
    NotFound,
    Business,
    Conflict,
    Unauthorized,
    Unexpected
}

public sealed record Error(string Code, string Description, ErrorType Type)
{
    public static readonly Error None =
        new(string.Empty, string.Empty, ErrorType.None);

    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Business(string code, string description) =>
        new(code, description, ErrorType.Business);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    public static Error Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);

    public static Error Unexpected(string code, string description) =>
        new(code, description, ErrorType.Unexpected);

    public override string ToString() => $"[{Type}] {Code}: {Description}";
}
