using System;
using System.Collections.Generic;
using System.Linq;
using Redakt.Core.Cache;
using Redakt.Data.Repository;
using Redakt.Model;

namespace Redakt.Core.Services
{
    public interface ISiteService
    {
        Site Get(string id);
        void Save(Site page);
    }

    public class SiteService : ISiteService
    {
        private readonly ISiteRepository _pageRepository;
        private readonly ICache _cache;

        public SiteService(ISiteRepository pageRepository, ICache cache)
        {
            _pageRepository = pageRepository;
            _cache = cache;
        }

        public Site Get(string id)
        {
            return _cache.GetOrSet(id, s => _pageRepository.GetAsync(s).Result);
        }

        public void Save(Site site)
        {
            _pageRepository.SaveAsync(site);
        }
    }
}
