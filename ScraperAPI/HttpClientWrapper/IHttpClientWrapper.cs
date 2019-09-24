using ScraperAPI.Enumerations;
using System.Threading.Tasks;

namespace ScraperAPI
{
    public interface IHttpClientWrapper
    {
        Task<string> GetHtmlViaHttpRequest(HttpRequestType methodType, string encodedUrl, string json = "");
        Task<byte[]> GetBytesViaHttpRequest(HttpRequestType methodType, string encodedUrl, string json = "");
    }
}
