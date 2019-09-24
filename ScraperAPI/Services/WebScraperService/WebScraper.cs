using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ScraperAPI.CustomExceptions;
using ScraperAPI.Enumerations;
using ScraperAPI.Extensions;
using ScraperAPI.Models;
using ScraperAPI.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ScraperAPI.InternalServices
{

    public class WebScraper : IWebScraper
    {
        private readonly IHtmlParserService _htmlParserService;
        private readonly IHttpClientWrapper _httpClient;        
        private readonly ILogger _logger;
        private readonly bool _storeDuplicateContactDetails;

        public WebScraper(IHtmlParserService htmlParserService, IHttpClientWrapper httpClient, ILogger<WebScraper> logger, IConfiguration config)
        {
            _htmlParserService = htmlParserService;
            _httpClient = httpClient;
            _logger = logger;

            _storeDuplicateContactDetails = config.GetValue<bool>("ScrapingOptions:StoreDuplicateCotactDetails");
        }

        public async Task<CompanyScraped> ScrapeUrlAsync(string originalUrl)
        {
            List<PageScrape> pagesScrapes = new List<PageScrape>();

            List<string> contactPageUrls = await GetContactPageUrls(originalUrl);

            //Even if the contact pages found are empty we will scrape the given Url
            contactPageUrls.Add(originalUrl);

            foreach (string contactPageUrl in contactPageUrls)
            {
                try
                {
                    ContactDetails contactDetails = await GetContactDetails(contactPageUrl);
                    if (contactDetails?.Emails == null && contactDetails?.PhoneNumbers == null)
                        continue;

                    if (!_storeDuplicateContactDetails)
                        contactDetails.RemoveDuplicateContactDetails(pagesScrapes);

                    pagesScrapes.Add(new PageScrape
                    {
                        ScrapedUrl = contactPageUrl,
                        ContactDetails = new ContactDetails
                        {
                            Emails = contactDetails.Emails.Count == 0 ? null : contactDetails.Emails,
                            PhoneNumbers = contactDetails.PhoneNumbers.Count == 0 ? null : contactDetails.PhoneNumbers
                        }
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error getting contact details from URL: {contactPageUrl} - {ex}");

                    continue;
                }
            }

            return new CompanyScraped
            {
                PageScrapes = pagesScrapes,
                Url = originalUrl
            };
        }        

        private async Task<ContactDetails> GetContactDetails(string contactPageUrl)
        {
            string pageContent = await _httpClient.GetHtmlViaHttpRequest(HttpRequestType.GET, contactPageUrl) ?? throw new HttpStatusCodeResponseException(HttpStatusCode.NotFound);

            List<string> emailStrings = _htmlParserService.ParseHtmlForEmailAddresses(pageContent) ?? throw new HttpStatusCodeResponseException(HttpStatusCode.NotFound);

            List<string> phoneNumbers = _htmlParserService.ParseHtmlForPhoneNumbers(pageContent);

            return new ContactDetails
            {
                Emails = emailStrings,
                PhoneNumbers = phoneNumbers
            };
        }

        private async Task<List<string>> GetContactPageUrls(string originalUrl)
        {
            string html = await _httpClient.GetHtmlViaHttpRequest(HttpRequestType.GET, originalUrl);

            List<string> contactPageUrls = _htmlParserService.ParseHtmlForContactLinks(html, originalUrl)
                .PrefixUrls(originalUrl); //Some href attribs are relative in source code but not to us. Prefix the original Url

            return contactPageUrls;
        }
    }
}
