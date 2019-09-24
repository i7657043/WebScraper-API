using ScraperAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ScraperAPI.Extensions
{
    public static class WebScraperExtensions
    {
        public static List<string> PrefixUrls(this List<string> contactMatches, string originalUrl)
        {
            List<string> list = contactMatches.Select(contactPageUrl =>
            {
                if (!contactPageUrl.ToLower().Contains("http") && !contactPageUrl.ToLower().Contains("https"))
                {
                    return $"{originalUrl}{contactPageUrl}";
                }                    

                return $"{contactPageUrl}";

            }).ToList();

            return list;
        }

        public static void RemoveDuplicateContactDetails(this ContactDetails contactDetails, List<PageScrape> pagesScrapes)
        {
            foreach (PageScrape pageScrape in pagesScrapes)
            {
                contactDetails?.Emails.ClearDuplicateContactDetails(pageScrape.ContactDetails.Emails);
                contactDetails?.PhoneNumbers.ClearDuplicateContactDetails(pageScrape.ContactDetails.PhoneNumbers);
            }
        }

        private static void ClearDuplicateContactDetails(this List<string> contactDetails, List<string> alreadyStoredContactDetails)
        {
            if (alreadyStoredContactDetails == null)
                return;

            List<int> indexsMarkedForDeletion = new List<int>();

            for (int j = 0; j < contactDetails.Count; j++)
            {
                if (alreadyStoredContactDetails.Any(x => x == contactDetails[j]))
                    indexsMarkedForDeletion.Add(j);
            }

            indexsMarkedForDeletion.ForEach(index => contactDetails.RemoveAt(index));
        }
    }
}
