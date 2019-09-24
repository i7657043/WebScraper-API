using System.Threading.Tasks;

namespace ScraperAPI.Services
{
    public interface ILinkPreviewService
    {
        Task<LinkPreviewResponse> GetLinkPreviewAsync(string url);
    }
}
