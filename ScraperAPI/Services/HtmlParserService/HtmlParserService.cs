using Microsoft.Extensions.Configuration;
using ScraperAPI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScraperAPI.InternalServices
{

    public class HtmlParserService : IHtmlParserService
    {
        private readonly IEnumerable<string> _contactPageWordList;
        private readonly bool _useContactPageWordList;
        public HtmlParserService(IConfiguration config)
        {
            _contactPageWordList = config.GetSection("ScrapingOptions:ContactPageWordList")
                .GetChildren()
                .ToList()
                .Select(x => x.Value)
                .ToList();

            _useContactPageWordList = config.GetValue<bool>("ScrapingOptions:UseContactPageFilter");
        }

        public List<string> ParseHtmlForContactLinks(string html, string originalUrl)
        {            
            Regex hrefMatcher = new Regex("<a\\s+(?:[^>]*?\\s+)?href=([\"'])(.*?)\\1");
            Regex externalUrlMatcher = new Regex(@"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})");

            MatchCollection hrefMatches = hrefMatcher.Matches(html);

            List<string> contactMatches = hrefMatches.Where(x => !externalUrlMatcher.IsMatch(x.Value) || x.Value.Contains(originalUrl))                
                .Select(regexMatch =>
                {
                    return regexMatch.Groups[2].Value;
                })
                .Distinct()
                .ToList();

            if (_useContactPageWordList)
                contactMatches = contactMatches.Where(contactPageUrl => _contactPageWordList.Any(y => contactPageUrl.ToLower().Contains(y.ToLower())))
                    .ToList(); 

            return contactMatches;
        }

        public List<string> ParseHtmlForEmailAddresses(string html)
        {
            Regex emailAddressMatcher = new Regex("(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\\])");
            
            List<string> emailAddresses = emailAddressMatcher.Matches(html)
                .Select(x => x.Value)
                .Distinct()
                .ToList();

            return emailAddresses;
        }

        public List<string> ParseHtmlForPhoneNumbers(string html)
        {
            Regex phoneNumberMatcher = new Regex(@"(.*)((?:0|\+?44)(?:\d\s?){9,10})");

            List<string> phoneNumbers = phoneNumberMatcher.Matches(html)
                .Select(x => x.Groups[2].Value.Trim().Replace(" ", ""))
                .Distinct()
                .ToList();

            return phoneNumbers;
        }
    }
}
