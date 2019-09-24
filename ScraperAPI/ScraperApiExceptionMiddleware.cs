using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ScraperAPI.CustomExceptions;
using ScraperAPI.ExceptionMiddleware;
using System;
using System.Threading.Tasks;

namespace ScraperAPI
{
    public class ScraperApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ILogger<ScraperApiExceptionMiddleware> _logger;

        public ScraperApiExceptionMiddleware(RequestDelegate next, IExceptionHandler exceptionHandler, ILogger<ScraperApiExceptionMiddleware> logger)
        {
            _exceptionHandler = exceptionHandler;
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (HttpStatusCodeResponseException ex)
            {
                _logger.LogError($"HTTP Status Code Response: {(int)ex.HttpStatusCode} Exception:  {ex}");

                await _exceptionHandler.HandleHttpStatusCodeResponseExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex}");

                await _exceptionHandler.HandleExceptionAsync(httpContext, ex);
            }
        }
    }
}
