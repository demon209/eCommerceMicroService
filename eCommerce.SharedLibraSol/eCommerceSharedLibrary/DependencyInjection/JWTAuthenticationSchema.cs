
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace eCommerceSharedLibrary.DependencyInjection
{
    // Cấu hình JWT
    public static class JWTAuthenticationSchema
    {
        public static IServiceCollection AddJWTAutheticationSchema(this IServiceCollection services, IConfiguration config)
        {
            // add JWT service
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(config["Authentication:Key"]!);
        var issuer = config["Authentication:Issuer"]!;
        var audience = config["Authentication:Audience"]!;

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true, // Bật xác thực thời gian hết hạn
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            RoleClaimType = ClaimTypes.Role //Vai trò người dùng
        };
    });

            return services;
        }
    }
}
