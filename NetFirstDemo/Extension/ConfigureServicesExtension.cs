using Microsoft.Extensions.DependencyInjection;
using NetFirstDemo.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NetFirstDemo.Data;
using Microsoft.EntityFrameworkCore;

namespace NetFirstDemo.Extension
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection AddTokenConfiguration(
            this IServiceCollection services, IConfiguration configuration)
        {
            TokenManagement tokenManagement = configuration.GetSection("tokenManagement").Get<TokenManagement>();
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret)),
                    ValidIssuer = tokenManagement.Issuer,
                    ValidAudience = tokenManagement.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });
            return services;
        }

        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "WebApiDemo",
                    Version = "v1"
                });
            });
            return services;
        }

        public static IServiceCollection AddDbContextConfiguration(this IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt =>
            {
                opt.UseInMemoryDatabase("TodoList");
            });
            return services;
        }
    }
}
