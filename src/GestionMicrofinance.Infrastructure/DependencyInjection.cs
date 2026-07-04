using GestionMicrofinance.Application.Common.Interfaces;
using GestionMicrofinance.Infrastructure.Persistence;
using GestionMicrofinance.Infrastructure.Persistence.Repositories;
using GestionMicrofinance.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionMicrofinance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddHttpContextAccessor();

        services.AddScoped<IPersonneRepository, PersonneRepository>();
        services.AddScoped<ICategorieRepository, CategorieRepository>();
        services.AddScoped<IOperationRepository, OperationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
