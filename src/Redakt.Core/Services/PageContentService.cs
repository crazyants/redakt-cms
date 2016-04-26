using System;
using System.Collections.Generic;
using System.Linq;
using Redakt.Core.Cache;
using Redakt.Data.Repository;
using Redakt.Model;

namespace Redakt.Core.Services
{
    public interface IPageContentService
    {
        PageContent Get(string id);
    }

    public class PageContentService: IPageContentService
    {
        private readonly ICache _cache;
        private readonly IPageContentRepository _pageContentRepository;

        public PageContentService(IPageContentRepository pageContentRepository, ICache cache)
        {
            _pageContentRepository = pageContentRepository;
            _cache = cache;
        }

        public PageContent Get(string id)
        {
            return _cache.GetOrSet(id, s => _pageContentRepository.GetAsync(s).Result);
        }
    }
}
