using eCommerce.Core.Entities;
using eCommerce.Core.Options;
using eCommerce.Core.Service;
using eCommerce.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection service, IConfiguration config)
    {
        service.AddScoped<ICleanerAvailabilityService, CleanerAvailabilityService>();
        service.AddScoped<IUsersService, UserService>();
        service.AddScoped<IServiceService, ServiceService>();
        service.AddScoped<ILocationService, LocationService>();
      service.AddScoped<INotificationService, NotificationService>();
        service.AddScoped<IReviewService, ReviewService>();
        service.AddScoped<IPaymentService, PaymentService>();
        service.AddScoped<IBookingService, BookingService>();
        service.AddScoped<IReportingService, ReportingService>();
        service.AddScoped<IDashboardService, DashboardService>();
        return service;
    }
}
