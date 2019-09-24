using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web;
using ScraperAPI.Services;
using ScraperAPI.Models;
using ScraperAPI.ResponseModels;
using ScraperAPI.Enumerations;

namespace ScraperAPI.Proxys
{
    public class ScraperServiceProxy : IScraperService
    {
        private readonly IHttpClientWrapper _client;

        public ScraperServiceProxy(IHttpClientWrapper client)
        {
            _client = client;
        }

        public async Task<ActionResult<ResponseBase<CompanyScraped>>> ScrapeUrlForContactDetailsAsync(string url)
        {
            string encodedUrl = $"https://localhost:5003/Scraper/{HttpUtility.UrlEncode(url)}";

            string result = await _client.GetHtmlViaHttpRequest(HttpRequestType.POST, encodedUrl);

            ResponseBase<CompanyScraped> response = JsonConvert.DeserializeObject<ResponseBase<CompanyScraped>>(result);

            return response;
        }        
    }
}
