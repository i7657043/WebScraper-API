using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using ScraperAPI.BLL;
using ScraperAPI.DAL;
using ScraperAPI.ExceptionMiddleware;
using ScraperAPI.InternalServices;
using ScraperAPI.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace ScraperAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IScraperProvider, ScraperProvider>()
                .AddSingleton<IScraperRepository, ScraperRepository>()
                .AddSingleton<IHttpClientWrapper, HttpClientWrapper>()
                .AddSingleton<ILinkPreviewService, LinkPreviewService>()
                .AddSingleton<IWebScraper, WebScraper>()
                .AddSingleton<IHtmlParserService, HtmlParserService>()
                .AddSingleton<IClearbitLogoService, ClearbitLogoService>()
                .AddSingleton<IExceptionHandler, ExceptionHandler>();

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new Info { Title = "Scraper API", Version = "v1.0" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Logging
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

            app.UseHttpsRedirection();

            app.ConfigureCustomExceptionMiddleware<ScraperApiExceptionMiddleware>();

            app.UseMvc();

            //Swagger API doc
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Scraper API");
            });
        }
    }
}
