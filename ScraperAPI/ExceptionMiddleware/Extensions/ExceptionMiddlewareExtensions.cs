using Microsoft.AspNetCore.Builder;

namespace ScraperAPI.ExceptionMiddleware
{

    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware<T>(this IApplicationBuilder app) 
            where T : class
        {
            app.UseMiddleware<T>();
        }        
    }
}
