using Microsoft.Extensions.Logging;
using ScraperAPI.Models;
using ScraperAPI.Services;
using System.Threading.Tasks;

namespace ScraperAPI.BLL
{
    class ScraperProvider : IScraperProvider
    {
        private readonly IWebScraper _webScraper;
        private readonly ILinkPreviewService _linkPreviewService;
        private readonly IClearbitLogoService _logoService;

        public ScraperProvider(IWebScraper webScraper, ILinkPreviewService linkPreviewService, IClearbitLogoService logoService)
        {
            _webScraper = webScraper;
            _linkPreviewService = linkPreviewService;
            _logoService = logoService;
        }

        public async Task<CompanyScraped> ScrapeUrlForContactDetailsAsync(string url)
        {
            CompanyScraped company = await _webScraper.ScrapeUrlAsync(url);

            company.Summary = await _linkPreviewService.GetLinkPreviewAsync(url);

            company.LogoBase64String = await _logoService.GetCompanyLogoAsBase64(url);

            return company;
        }
    }
}
