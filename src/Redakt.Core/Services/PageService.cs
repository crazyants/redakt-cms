using System;
using System.Collections.Generic;
using System.Linq;
using Redakt.Core.Cache;
using Redakt.Data.Repository;
using Redakt.Model;

namespace Redakt.Core.Services
{
    public interface IPageService
    {
        Page Get(string id);
        IEnumerable<Page> GetAncestors(Page page);
        void Move(Page page, Page newParent);
        void Save(Page page);
    }

    public class PageService : IPageService
    {
        private readonly ICache _cache;
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository, ICache cache)
        {
            _pageRepository = pageRepository;
            _cache = cache;
        }

        public Page Get(string id)
        {
            return _cache.GetOrSet(id, s => _pageRepository.GetAsync(s).Result);
        }

        public IEnumerable<Page> GetAncestors(Page page)
        {
            return Get(page.AncestorIds);
        }

        public void Move(Page page, Page newParent)
        {
            var ancestors = newParent.AncestorIds.ToList();
            ancestors.Add(newParent.Id);
            page.AncestorIds = ancestors;

            Save(page);
        }

        public void Save(Page page)
        {
            _pageRepository.SaveAsync(page);
        }

        public void SaveAndPublish(Page page)
        {
            page.PublishedAt = DateTime.UtcNow;
            page.PublishedByUserId = RedaktContext.Current.User.Id;

            Save(page);
        }

        private IEnumerable<Page> Get(IEnumerable<string> ids)
        {
            var pages = ids.Select(id => _cache.GetOrDefault<Page>(id));
            if (pages.Count() == ids.Count()) return pages;

            pages = _pageRepository.GetAsync(ids).Result;
            foreach (var page in pages) _cache.Set(page);
            return pages;
        }
    }
}
