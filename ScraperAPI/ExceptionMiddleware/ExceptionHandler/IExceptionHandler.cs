using Microsoft.AspNetCore.Http;
using ScraperAPI.CustomExceptions;
using System;
using System.Threading.Tasks;

namespace ScraperAPI.ExceptionMiddleware
{
    public interface IExceptionHandler
    {
        Task HandleHttpStatusCodeResponseExceptionAsync(HttpContext context, HttpStatusCodeResponseException exception);
        Task HandleExceptionAsync(HttpContext context, Exception exception);
    }
}
