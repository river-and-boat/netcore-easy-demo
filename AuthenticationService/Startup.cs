using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using UserService.Data.Repository;
using UserService.Extension;
using UserService.Filter;
using UserService.Middleware;
using UserService.Service;
using UserService.Service.Authentication;

namespace AuthenticationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTokenConfiguration(Configuration);
            services.AddDatabaseConfiguration(Configuration);
            services.AddControllers(option =>
            {
                option.Filters.Add<ResponseWrapperFilter>();
            }).AddNewtonsoftJson(option => {
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddScoped<IAuthenticationService, JwtAuthenticationService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<UserManagementService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
