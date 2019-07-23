﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Configuration
{
    public static class ConfigureConnection
    {
        public static IServiceCollection AddConnectionProvider
            (this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<MyContext>(opt =>
            opt.UseSqlServer(
                configuration.GetConnectionString("Identity"),
                b => b.MigrationsAssembly("Project")));

            return services;
        }
    }
}
