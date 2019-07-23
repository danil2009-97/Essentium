using Abc.Auth.Interfaces;
using Abc.Auth.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Project.Core.EF;
using Project.Core.Repositories;
using Project.Data.Enteties;
using Project.Data.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Configuration
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddIdentity(
            this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<int>>(o =>
            {
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<MyContext>();

            return services;
        }

        public static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IJwtGenerator, JwtGenerator>()
                .AddTransient<IAuthService, AuthService>();

            return services;
        }

        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", new Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials()
                    .Build());
            });

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddMail(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMailKit(optionBuilder =>
            {
                optionBuilder.UseMailKit(new MailKitOptions()
                {
                    Server = configuration["Server"],
                    Port = Convert.ToInt32(configuration["Port"]),
                    SenderName = configuration["SenderName"],
                    SenderEmail = configuration["SenderEmail"],

                    Account = configuration["Account"],
                    Password = configuration["Password"],
                    Security = true
                });
            });

            return services;
        }

    }
}
