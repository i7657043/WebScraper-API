using ScraperAPI.Models;
using System.Threading.Tasks;

namespace ScraperAPI.Services
{
    public interface IWebScraper
    {
        Task<CompanyScraped> ScrapeUrlAsync(string url);
    }

}
