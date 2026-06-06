namespace UgeFactura.Common.Primitives
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        // Métodos estáticos de entrada (Estilo Fluent)
        public static Result Ok() => new(true, string.Empty);
        public static Result Fail(string error) => new(false, error);

        public static Result<T> Ok<T>(T value) => Result<T>.Ok(value);
        public static Result<T> Fail<T>(string error) => Result<T>.Fail(error);
    }

    public class Result<T> : Result
    {
        private readonly T? _value;

        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException($"No se puede acceder al valor de un resultado fallido. Error: {Error}");

        private Result(T? value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        // Métodos específicos para la versión genérica
        public static Result<T> Ok(T value) => new(value, true, string.Empty);
        public new static Result<T> Fail(string error) => new(default, false, error);

        // Operador implícito para que puedas hacer: return objeto;
        public static implicit operator Result<T>(T value) => Ok(value);
    }
}
