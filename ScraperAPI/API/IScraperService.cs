using Microsoft.AspNetCore.Mvc;
using ScraperAPI.Models;
using ScraperAPI.ResponseModels;
using System.Threading.Tasks;

namespace ScraperAPI.Services
{
    public interface IScraperService
    {
        Task<ActionResult<ResponseBase<CompanyScraped>>> ScrapeUrlForContactDetailsAsync(string url);
    }
}
