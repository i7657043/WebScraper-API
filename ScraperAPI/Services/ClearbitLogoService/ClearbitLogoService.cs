using Microsoft.Extensions.Logging;
using ScraperAPI.Enumerations;
using System;
using System.Threading.Tasks;
using System.Web;

namespace ScraperAPI.Services
{
    public class ClearbitLogoService : IClearbitLogoService
    {
        private readonly ILogger<ClearbitLogoService> _logger;
        private readonly IHttpClientWrapper _httpClient;

        public ClearbitLogoService(ILogger<ClearbitLogoService> logger, IHttpClientWrapper client)
        {
            _logger = logger;
            _httpClient = client;
        }

        public async Task<string> GetCompanyLogoAsBase64(string url)
        {
            byte[] responseBytes = await _httpClient.GetBytesViaHttpRequest(HttpRequestType.GET, $"http://logo.clearbit.com/{HttpUtility.UrlEncode(url)}");

            //data:image/png;base64
            return Convert.ToBase64String(responseBytes);
        }
    }
}
