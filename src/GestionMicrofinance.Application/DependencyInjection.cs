using GestionMicrofinance.Application.Auth;
using GestionMicrofinance.Application.Categories;
using GestionMicrofinance.Application.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace GestionMicrofinance.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICategorieService, CategorieService>();
        services.AddScoped<IOperationService, OperationService>();

        return services;
    }
}
