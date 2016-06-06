using System;
using System.Collections.Generic;
using System.Linq;
using Redakt.Core.Cache;
using Redakt.Data.Repository;
using Redakt.Model;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Redakt.Core.Services
{
    public interface IPageService
    {
        Task<Page> Get(string id);
        Task<Page> GetParent(Page page);
        Task<IList<Page>> GetAncestors(Page page);
        Task<IList<Page>> GetChildren(string pageId);
        Task<IList<Page>> GetDescendants(string pageId);
        Task Move(Page page, Page newParent);
        Task Save(Page page);
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

        public Task<Page> Get(string id)
        {
            return _cache.AddOrGetExistingAsync(id, s => _pageRepository.GetAsync(s));
        }

        public Task<IList<Page>> GetAncestors(Page page)
        {
            return Get(page.AncestorIds);
        }

        public Task<Page> GetParent(Page page)
        {
            return Get(page.ParentId);
        }

        public Task<IList<Page>> GetChildren(string pageId)
        {
            return _pageRepository.GetChildrenAsync(pageId);
        }

        public Task<IList<Page>> GetDescendants(string pageId)
        {
            return _pageRepository.GetDescendantsAsync(pageId);
        }

        public async Task Move(Page page, Page newParent)
        {
            var oldParentId = page.ParentId;

            page.SetParent(newParent);
            await _pageRepository.SaveAsync(page);

            if (oldParentId != null && !await _pageRepository.HasChildrenAsync(oldParentId).ConfigureAwait(false))
            {
                await _pageRepository.SetHasChildrenAsync(page.ParentId, false).ConfigureAwait(false);
            }

            if (newParent != null && !newParent.HasChildren)
            {
                await _pageRepository.SetHasChildrenAsync(page.ParentId, true);
            }
        }

        public Task Save(Page page)
        {
            if (page.IsNew() && page.ParentId != null)
            {
                return Task.WhenAll(_pageRepository.SetHasChildrenAsync(page.ParentId, true), _pageRepository.SaveAsync(page));
            }

            return _pageRepository.SaveAsync(page);
        }

        public Task SaveAndPublish(Page page)
        {
            page.PublishedAt = DateTime.UtcNow;
            page.PublishedByUserId = RedaktContext.Current.User.Id;

            return Save(page);
        }

        private async Task<IList<Page>> Get(IEnumerable<string> ids)
        {
            var cachedPages = _cache.Get<Page>(ids).ToList();
            if (cachedPages.Count() == ids.Count()) return cachedPages;

            var pages = await _pageRepository.GetAsync(ids);
            _cache.Set<Page>(pages);
            return pages;
        }
    }
}
