using ScraperAPI.Services;
using System.Collections.Generic;

namespace ScraperAPI.Models
{
    public class CompanyScraped
    {
        public string Url { get; set; }
        public List<PageScrape> PageScrapes { get; set; }
        public LinkPreviewResponse Summary {get;set;}
        public string LogoBase64String { get; set; }
    }

}
