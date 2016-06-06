using System;
using System.Collections.Generic;
using System.Linq;
using Redakt.Core.Cache;
using Redakt.Data.Repository;
using Redakt.Model;
using System.Threading.Tasks;

namespace Redakt.Core.Services
{
    public interface ISiteService
    {
        Task<IList<Site>> GetAll();
        Task<Site> Get(string id);
        Task Save(Site page);
    }

    public class SiteService : ISiteService
    {
        private readonly ISiteRepository _siteRepository;
        private readonly ICache _cache;

        public SiteService(ISiteRepository siteRepository, ICache cache)
        {
            _siteRepository = siteRepository;
            _cache = cache;
        }

        public Task<Site> Get(string id)
        {
            return _cache.AddOrGetExistingAsync(id, s => _siteRepository.GetAsync(s));
        }

        public Task<IList<Site>> GetAll()
        {
            return _siteRepository.FindAsync(s => true);
        }

        public Task Save(Site site)
        {
            return _siteRepository.SaveAsync(site);
        }
    }
}
