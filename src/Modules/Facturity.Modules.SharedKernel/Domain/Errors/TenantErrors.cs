namespace Facturity.Modules.SharedKernel.Domain;

public static class TenantErrors
{
    public static readonly Error IdInvalido =
        Error.Validation("Tenant.IdInvalido", "El id del tenant es inválido.");
}