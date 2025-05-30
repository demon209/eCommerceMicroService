namespace ApiGateway.Presentation.Middleware
{
    // Gán header = Signed cho Api_Gateway mỗi request
    public class AttachSignatureToRequest(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers["Api-Gateway"] = "Signed";
            await next(context);
        }
    }
}
