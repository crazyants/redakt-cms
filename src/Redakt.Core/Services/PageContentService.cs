using System;
using System.Collections.Generic;
using System.Linq;
using Redakt.Core.Cache;
using Redakt.Data.Repository;
using Redakt.Model;
using System.Threading.Tasks;

namespace Redakt.Core.Services
{
    public interface IPageContentService
    {
        Task<PageContent> Get(string id);
        Task Save(PageContent content);
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

        public Task<PageContent> Get(string id)
        {
            return _cache.AddOrGetExistingAsync(id, s => _pageContentRepository.GetAsync(s));
        }

        public Task Save(PageContent content)
        {
            return _pageContentRepository.SaveAsync(content);
        }
    }
}
