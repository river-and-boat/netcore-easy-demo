using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.Domain.Authentication;
using UserService.Data;
using Microsoft.EntityFrameworkCore;

namespace UserService.Extension
{
    public static class ConfigureServicesExtension
    {
        public static void AddTokenConfiguration(
            this IServiceCollection services, IConfiguration configuration)
        {
            TokenManager tokenManagement = configuration.GetSection("Token").Get<TokenManager>();
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
        }

        public static void AddDatabaseConfiguration(
            this IServiceCollection services, IConfiguration configuration)
        {
            string dbConnection = configuration.GetConnectionString("Mysql");
            services.AddDbContext<UserRoleDbContext>(option =>
            {
                option.UseMySql(dbConnection, ServerVersion.AutoDetect(dbConnection));
            });
        }
    }
}
