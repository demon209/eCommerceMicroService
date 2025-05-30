using System.Net;
using System.Text.Json;
using eCommerceSharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceSharedLibrary.Middleware
{
        //Middleware xử lý ngoại lệ toàn cục
        // Ghi log lỗi để kiểm tra
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Declare default variables
            string message = "sorry, internal server error occured. Kindly try again";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";
            try
            {
                await next(context);

                // check if Response is too many request //429 status code.
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many request!";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, message, statusCode);
                }

                // if Response is UnAuthorized // 401 status code
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "You are not authorized to access.";
                    statusCode = (int)(StatusCodes.Status401Unauthorized);
                    await ModifyHeader(context, title, message, statusCode);
                }

                // if Response is Forbidden //403 status code
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Access";
                    message = "You are not allowed/required to access";
                    statusCode = (int)(StatusCodes.Status403Forbidden);
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex) 
            {
                // Log Original Exceptions /File, Debugger, Console
                LogException.LogExceptions(ex);

                //check if Exception is Timeout // 408 status code
                if(ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of time";
                    message = "Request timeout....try again";
                    statusCode = (int)(StatusCodes.Status408RequestTimeout);

                }

                // if ex is caught
                // if none of the exceptions then do the default
                await ModifyHeader(context,title, message, statusCode);
            
            
            }

        }

        //Trả về lỗi dạng Json
        private async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            // display scary-free message to client
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = title
            }), CancellationToken.None);
            return;
        }
    }
}
