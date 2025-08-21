using eCommerce.Core.Service;
using eCommerce.Core.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection service)
    {
        service.AddTransient<IUsersService, UserService>();

        return service;
    }
}
