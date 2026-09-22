namespace Facturity.Modules.SharedKernel.Domain.Extensions;

public sealed class ErrorExtension
{
    public static readonly Error Generico =
        Error.Unexpected("ErrorGenerico", "Ha ocurrido un error.");
}