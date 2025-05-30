using eCommerceSharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace eCommerceSharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services, IConfiguration config, string fileName)
            where TContext : DbContext
        {
            // Add Generic Database context
            // Kết nối tới Oracle bằng ConnectStr
            services.AddDbContext<TContext>(option => option.UseOracle(
                config
                .GetConnectionString("eCommerceConnection")));

            // configure serilog logging
            // Cấu hình sẻilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss:fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();
            // Add JWT authentication Schema
            JWTAuthenticationSchema.AddJWTAutheticationSchema(services, config);
            return services;
        }


    public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            //Use middleware global Exception 
            //Sử dụng middleware tổng quát /Middleware/GlobalException.cs
            app.UseMiddleware<GlobalException>();

            // Register middleware to block all outsiders API calls
            // Chỉ cho gọi API qua ApiGateway
            //app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}