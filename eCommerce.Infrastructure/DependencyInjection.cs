using Dapper;
using eCommerce.Core.Entities;
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

        //SqlMapper.AddTypeHandler(new DateOnlyHandler());
        //SqlMapper.AddTypeHandler(new TimeOnlyHandler());
        services.Configure<JwtOptions>(config.GetSection("Jwt"));

        // Data
        services.AddTransient<DapperDbContext>();

        services.AddTransient<IUsersRepository, UsersRepository>();
        services.AddTransient<IjwtRepository, JwtService>();
        services.AddTransient<IServiceRepository, ServiceRepository>();
        services.AddTransient<ILocationRepository, LocationRepository>();
        services.AddTransient<INotificationRepository, NotificationRepository>();
        services.AddTransient<ICleanerAvailabilityRepository, CleanerAvailabilityRepository>();
        services.AddTransient<IReviewRepository, ReviewRepository>();
        services.AddTransient<IPaymentRepository, PaymentRepository>();
        services.AddTransient<IBookingRepository, BookingRepository>();
        services.AddTransient<IReportRepository, ReportRepository>();
        return services;
    }
}

