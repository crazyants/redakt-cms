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
    public interface IPageTypeService
    {
        Task<PageType> Get(string id);
        Task Save(PageType pageType);
    }

    public class PageTypeService : IPageTypeService
    {
        private readonly ICache _cache;
        private readonly IPageTypeRepository _pageTypeRepository;

        public PageTypeService(IPageTypeRepository pageTypeRepository, ICache cache)
        {
            _pageTypeRepository = pageTypeRepository;
            _cache = cache;
        }

        public Task<PageType> Get(string id)
        {
            return _cache.AddOrGetExistingAsync(id, s => _pageTypeRepository.GetAsync(s));
        }

        public Task Save(PageType pageType)
        {
            return _pageTypeRepository.SaveAsync(pageType);
        }
    }
}
