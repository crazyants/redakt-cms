using Redakt.Data.Test.Repository;
using Redakt.Model;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;

namespace Redakt.Data.Test
{
    public class DbInit: IDbInit
    {
        private readonly ILogger<DbInit> _logger;

        public DbInit(ILogger<DbInit> logger)
        {
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogDebug("Starting database initialization.");

            this.PopulateTestData();
        }

        private void PopulateTestData()
        {
            _logger.LogDebug("Populating test data.");

            var pageType = new PageType
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = "Content Page Type",
                IconClass = "md md-content-copy"
            };
            PageTypeRepository.Collection.Add(pageType);

            // Pages
            var homePage1 = new Page
            {
                Id = Guid.NewGuid().ToString("N"),
                PageTypeId = pageType.Id,
                HasChildren = true,
                Name = "Redakt Home"
            };
            var homePage2 = new Page
            {
                Id = Guid.NewGuid().ToString("N"),
                PageTypeId = pageType.Id,
                HasChildren = true,
                Name = "Carvellis Home"
            };

            PageRepository.Collection.Add(homePage1);
            PageRepository.Collection.Add(homePage2);
            PageRepository.Collection.AddRange(CreatePageStructure(homePage1, 10, 5, 3));
            PageRepository.Collection.AddRange(CreatePageStructure(homePage2, 6, 8));

            // Sites
            SiteRepository.Collection.Add(new Site
            {
                Id = Guid.NewGuid().ToString("N"),
                HomePageId = homePage1.Id,
                Name = "Redakt CMS"
            });
            SiteRepository.Collection.Add(new Site
            {
                Id = Guid.NewGuid().ToString("N"),
                HomePageId = homePage2.Id,
                Name = "Carvellis Web Development"
            });
        }

        private List<Page> CreatePageStructure(Page parent, params int[] levels)
        {
            var list = new List<Page>();
            for (int i = 0; i < levels[0]; i++)
            {
                var page = new Page
                {
                    Id = Guid.NewGuid().ToString("N"),
                    PageTypeId = parent.PageTypeId,
                    HasChildren = levels.Count() > 1,
                    Name = "Page " + i
                };
                page.SetParent(parent);
                list.Add(page);

                if (levels.Count() > 1) list.AddRange(CreatePageStructure(page, levels.Skip(1).ToArray()));
            }
            return list;
        }
    }
}
