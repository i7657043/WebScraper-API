using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ScraperAPI.DAL
{
    //Not yet needed
    public class ScraperRepository : IScraperRepository
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        public ScraperRepository(ILogger<IScraperRepository> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
    }
}
