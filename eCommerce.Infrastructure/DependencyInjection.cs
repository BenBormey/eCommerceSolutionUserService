using eCommerce.Core.Options;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.Service;
using eCommerce.Infrastructure.DbContext;
using eCommerce.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastruction(this IServiceCollection services, IConfiguration config)
    {
        // Options
        services.Configure<JwtOptions>(config.GetSection("Jwt"));

        // Data
        services.AddTransient<DapperDbContext>();

        // Repositories/Services
        services.AddTransient<IUsersRepository, UsersRepository>();
        services.AddTransient<IjwtRepository, JwtService>();

        return services;
    }
}
