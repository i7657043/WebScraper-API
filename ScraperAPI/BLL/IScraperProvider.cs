using ScraperAPI.Models;
using System.Threading.Tasks;

namespace ScraperAPI.BLL
{
    public interface IScraperProvider
    {
        Task<CompanyScraped> ScrapeUrlForContactDetailsAsync(string url);
    }
}
