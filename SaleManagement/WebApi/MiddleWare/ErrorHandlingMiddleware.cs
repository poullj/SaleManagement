
using Newtonsoft.Json;
using Serilog;
using Shared.Types;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.MiddleWare
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ContextCorrelator _contextCorrelator;

        public ErrorHandlingMiddleware(RequestDelegate next, ContextCorrelator contextCorrelator)
        {
            _next = next;
            _contextCorrelator = contextCorrelator;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            string correlationId;
            if (httpContext.Request.Headers.TryGetValue(HttpHeaderNames.CORRELATIONID, out var correlationIds))
            {
                correlationId = correlationIds.First();
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                Log.Information("No CORRELATIONID received from Client - new {CORRELATIONID} is created", correlationId);
            }

            var action = httpContext.Request.Method + httpContext.Request.Path.Value;
            
            try
            {
                if (_contextCorrelator == null)
                {
                    Log.Error("contextCorrelator is null - not able to store correlation id for later log property - pls fix DI");
                }

                using (_contextCorrelator?.BeginCorrelationScope(HttpHeaderNames.CORRELATIONID, correlationId))
                {
                    Log.Debug("Action: {action} called with correlationID {CORRELATIONID}", action, correlationId);
                    await _next(httpContext);
                }

            }
            catch (Exception e)
            {
                await HandleControllerException(httpContext, e);
            }
            finally
            {
                Log.Debug("Action: {action} done with correlationID {CORRELATIONID}", action, correlationId);
            }
        }


        private static async Task HandleControllerException(HttpContext httpContext, Exception e)
        {
            var exceptionString = e.ToString();
            Log.Error(e, $"Request Error - internal server errror with exception");
            if (httpContext.Response.HasStarted == false)
            {
                if (typeof(ExpectedException).IsAssignableFrom(e.GetType()))
                {
                    ExpectedException expectedException = (ExpectedException)e;
                    exceptionString = JsonConvert.SerializeObject(
                        new
                        {
                            expectedException.Headline,
                            expectedException.Message,
                            expectedException.Detail
                        });
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    Log.Warning(e, exceptionString);
                }
                else
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    Log.Error($"Expected exception: {exceptionString}");
                }
                await httpContext.Response.WriteAsync(exceptionString);
            }
            else
            {
                Log.Error("Exception {@exception} occurred after response was started", e);
            }
        }
    }
}