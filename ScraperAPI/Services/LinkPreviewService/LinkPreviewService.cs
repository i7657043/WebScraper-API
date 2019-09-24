using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ScraperAPI.Enumerations;
using System.Threading.Tasks;

namespace ScraperAPI.Services
{
    public class LinkPreviewService : ILinkPreviewService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly string apiKey;

        public LinkPreviewService(IHttpClientWrapper httpClient, IConfiguration config)
        {
            _httpClient = httpClient;

            apiKey = config.GetValue<string>("LinkPreviewApiKey");
        }

        public async Task<LinkPreviewResponse> GetLinkPreviewAsync(string url)
        {
            string result = await _httpClient.GetHtmlViaHttpRequest(HttpRequestType.POST, $"http://api.linkpreview.net/?key={apiKey}&q={url}");
            
            return JsonConvert.DeserializeObject<LinkPreviewResponse>(result);
        }
    }
}
