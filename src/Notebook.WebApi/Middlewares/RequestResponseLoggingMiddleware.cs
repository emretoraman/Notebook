using Microsoft.AspNetCore.Http;
using Serilog;
using System.IO;
using System.Threading.Tasks;

namespace Notebook.WebApi
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDiagnosticContext _diagnosticContext;

        public RequestResponseLoggingMiddleware(RequestDelegate next, IDiagnosticContext diagnosticContext)
        {
            _next = next;
            _diagnosticContext = diagnosticContext;
        }

        public async Task Invoke(HttpContext context)
        {
            _diagnosticContext.Set("RequestUri", $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}{context.Request.Path}{context.Request.QueryString}");

            context.Request.EnableBuffering();
            _diagnosticContext.Set("RequestBody", await ReadStream(context.Request.Body, true));

            await SetResponse(context);
        }

        private static async Task<string> ReadStream(Stream stream, bool shouldRead)
        {
            stream.Seek(0, SeekOrigin.Begin);
            if (!shouldRead)
            {
                return string.Empty;
            }

            string text = await new StreamReader(stream).ReadToEndAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return text;
        }

        private async Task SetResponse(HttpContext context)
        {
            using MemoryStream responseBody = new();

            Stream originalResponseBody = context.Response.Body;
            context.Response.Body = responseBody;

            await _next(context);

            _diagnosticContext.Set("ResponseLength", context.Response.Body.Length);

            bool shouldRead = !context.Request.Path.Value.StartsWith("/swagger");
            _diagnosticContext.Set("ResponseBody", await ReadStream(context.Response.Body, shouldRead));

            await responseBody.CopyToAsync(originalResponseBody);
        }
    }
}