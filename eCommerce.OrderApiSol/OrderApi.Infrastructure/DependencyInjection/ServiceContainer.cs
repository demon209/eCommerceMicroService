using eCommerceSharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interface;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.Repositories;

namespace OrderApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config) 
        {
            // Add database connecttivity
            // Add authentication schema
            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, config, config["MySerilog: FileName"]!);

            // Create Dependency Injection 
            //4:09
            services.AddScoped<IOrder, OrderRepository>();
            return services;

        }
        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register middleware such as:
            //Global Exception -> handle external errors
            //ListenToApiGateway -> Block all outsiders call
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
