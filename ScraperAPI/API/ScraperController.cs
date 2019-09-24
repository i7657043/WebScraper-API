using Microsoft.AspNetCore.Mvc;
using ScraperAPI.BLL;
using ScraperAPI.CustomExceptions;
using ScraperAPI.Extensions;
using ScraperAPI.Models;
using ScraperAPI.ResponseModels;
using ScraperAPI.Services;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ScraperAPI.API
{
    [Route("Scraper")]
    public class ScraperController : Controller, IScraperService
    {
        private readonly IScraperProvider _scraperProvider;

        public ScraperController(IScraperProvider scraperProvider)
        {
            _scraperProvider = scraperProvider;
        }



        [ProducesResponseType((int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.NotFound)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.InternalServerError)]
        [HttpPost]
        [Route("{url}")]
        public async Task<ActionResult<ResponseBase<CompanyScraped>>> ScrapeUrlForContactDetailsAsync(string url)
        {
            Regex urlMatcher = new Regex(@"(https?)");

            if (!urlMatcher.IsMatch(url))
                throw new HttpStatusCodeResponseException(HttpStatusCode.BadRequest);

            url = HttpUtility.UrlDecode(url).RemoveTrailingSlash();            
            
            CompanyScraped result = await _scraperProvider.ScrapeUrlForContactDetailsAsync(url);

            return new ResponseBase<CompanyScraped>(result);
        }
    }
}