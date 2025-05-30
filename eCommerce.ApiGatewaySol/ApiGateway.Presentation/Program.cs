using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using eCommerceSharedLibrary.DependencyInjection;
using ApiGateway.Presentation.Middleware;
using Ocelot.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5003, listenOptions =>
    {
        listenOptions.UseHttps("/https/aspnetapp.pfx", "123456");
    });
});
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
// ??ng kí ocelot
builder.Services.AddOcelot().AddCacheManager(x => x.WithDictionaryHandle());
// C?u hình xác th?c Jwt
JWTAuthenticationSchema.AddJWTAutheticationSchema(builder.Services, builder.Configuration);
//Cors
builder.Services.AddCors(options =>
{   
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors();
app.UseMiddleware<AttachSignatureToRequest>();
app.UseOcelot().Wait();
app.MapGet("/", () => "Hello HTTPS World!");

app.Run();
