using System.Threading.Tasks;

namespace ScraperAPI.Services
{
    public interface IClearbitLogoService
    {
        Task<string> GetCompanyLogoAsBase64(string url);
    }

}
