 using NLog;

namespace ClinicBooking.Api.Common { 

    public class CorrelationIdMiddleware : IMiddleware
    {
        private const string Header = "X-Correlation-ID";
        public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
        {
            var corrId = ctx.Request.Headers.TryGetValue(Header, out var v) && !string.IsNullOrWhiteSpace(v)
                ? v.ToString() : Guid.NewGuid().ToString("N");

            NLog.MappedDiagnosticsLogicalContext.Set("CorrelationId", corrId);
            ctx.Response.Headers[Header] = corrId;

            try { await next(ctx); }
            finally { NLog.MappedDiagnosticsLogicalContext.Remove("CorrelationId"); }
        }
    }

}
