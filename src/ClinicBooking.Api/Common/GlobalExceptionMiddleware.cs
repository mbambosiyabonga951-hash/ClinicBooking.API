using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Net;
using System.Net.Http;
namespace ClinicBooking.Application.Providers
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
        {
            try { await next(ctx); }
            catch (ValidationException vex)
            {
                Logger.Warn(vex, "Validation failed");
                var pd = new ProblemDetails
                {
                    Title = "Validation Failed",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = "See error details.",
                    Type = "https://httpstatuses.com/400"
                };
                pd.Extensions["errors"] = vex.Errors;
                ctx.Response.StatusCode = pd.Status!.Value;
                await ctx.Response.WriteAsJsonAsync(pd);
            }
            catch (NotFoundException nf)
            {
                Logger.Info(nf, "Resource not found");
                var pd = new ProblemDetails
                {
                    Title = "Not Found",
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = nf.Message,
                    Type = "https://httpstatuses.com/404"
                };
                ctx.Response.StatusCode = pd.Status!.Value;
                await ctx.Response.WriteAsJsonAsync(pd);
            }
            catch (ConflictException cf)
            {
                Logger.Warn(cf, "Conflict");
                var pd = new ProblemDetails
                {
                    Title = "Conflict",
                    Status = (int)HttpStatusCode.Conflict,
                    Detail = cf.Message,
                    Type = "https://httpstatuses.com/409"
                };
                ctx.Response.StatusCode = pd.Status!.Value;
                await ctx.Response.WriteAsJsonAsync(pd);
            }
            catch (Exception ex)
            {
                // Last-resort: log with stack trace
                Logger.Error(ex, "Unhandled exception");
                var pd = new ProblemDetails
                {
                    Title = "Server Error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = "Unexpected error. Try again later.",
                    Type = "https://httpstatuses.com/500"
                };
                ctx.Response.StatusCode = pd.Status!.Value;
                await ctx.Response.WriteAsJsonAsync(pd);
            }
        }
    }

}
