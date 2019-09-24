using System.Collections.Generic;

namespace ScraperAPI.Services
{
    public interface IHtmlParserService
    {
        List<string> ParseHtmlForContactLinks(string html, string url);
        List<string> ParseHtmlForEmailAddresses(string html);
        List<string> ParseHtmlForPhoneNumbers(string html);
    }

}
