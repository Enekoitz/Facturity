using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Facturity.Modules.Auth.Features;

public static class DependencyInjection
{
    public static void AddAuthFeatures(this IServiceCollection services)
    {
        services.AddScoped<RefreshTokenIssuer>();
        services.AddScoped<ICommandHandler<RegisterCommand, Result<RegisterResult>>, RegisterHandler>();
        services.AddScoped<ICommandHandler<LoginCommand, Result<LoginResult>>, LoginHandler>();
        services.AddScoped<ICommandHandler<RefreshCommand, Result<RefreshResult>>, RefreshHandler>();
        services.AddScoped<ICommandHandler<LogoutCommand, Result>, LogoutHandler>();
    }
}