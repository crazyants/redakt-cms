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

            //var numberFieldEditor = new FieldType
            //{

            //};
            var numberFieldType = new FieldType
            {
                Name = "Numeric",
                FieldEditorId = "479a9f0acddd4af3bb3ff98d492f0e1a"
            };
            var stringFieldType = new FieldType
            {
                Name = "Textstring",
                FieldEditorId = "15317e3a67044ee5af7ccba1e9537cde"
            };
            var areaFieldType = new FieldType
            {
                Name = "Textarea",
                FieldEditorId = "15317e3a67044ee5af7ccba1e9537cde",
                FieldEditorSettings = new { IsTextArea = true }
            };
            var rtfFieldType = new FieldType
            {
                Name = "Rich text editor",
                FieldEditorId = "adde787fb7e746f69dbef237e1be80f8"
            };
            FieldTypeRepository.Collection.Add(numberFieldType);
            FieldTypeRepository.Collection.Add(stringFieldType);
            FieldTypeRepository.Collection.Add(areaFieldType);
            FieldTypeRepository.Collection.Add(rtfFieldType);

            var contentPageType = new PageType
            {
                Name = "Content Page",
                IconClass = "md md-content-copy"
            };
            contentPageType.Fields.Add(new FieldDefinition { Key = "title", Label = "Titel", FieldTypeId = stringFieldType.Id });
            contentPageType.Fields.Add(new FieldDefinition { Key = "quantity", Label = "Hoeveelheid", FieldTypeId = numberFieldType.Id });
            contentPageType.Fields.Add(new FieldDefinition { Key = "intro", Label = "Introtekst", FieldTypeId = areaFieldType.Id });
            contentPageType.Fields.Add(new FieldDefinition { Key = "body", Label = "Body tekst", FieldTypeId = rtfFieldType.Id });
            PageTypeRepository.Collection.Add(contentPageType);

            var homePageType = new PageType
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = "Home Page",
                IconClass = "md md-home"
            };
            homePageType.Fields.Add(new FieldDefinition { Key = "intro", Label = "Introtekst", FieldTypeId = areaFieldType.Id });
            homePageType.Fields.Add(new FieldDefinition { Key = "body", Label = "Body tekst", FieldTypeId = rtfFieldType.Id });
            PageTypeRepository.Collection.Add(homePageType);

            // Pages
            var homePage1 = new Page
            {
                PageTypeId = homePageType.Id,
                HasChildren = true,
                Name = "Redakt Home"
            };
            var homePage2 = new Page
            {
                PageTypeId = homePageType.Id,
                HasChildren = true,
                Name = "Carvellis Home"
            };

            PageRepository.Collection.Add(homePage1);
            PageRepository.Collection.Add(homePage2);
            PageRepository.Collection.AddRange(CreatePageStructure(homePage1, contentPageType, 10, 5, 3));
            PageRepository.Collection.AddRange(CreatePageStructure(homePage2, contentPageType, 6, 8));

            // Sites
            SiteRepository.Collection.Add(new Site
            {
                HomePageId = homePage1.Id,
                Name = "Redakt CMS"
            });
            SiteRepository.Collection.Add(new Site
            {
                HomePageId = homePage2.Id,
                Name = "Carvellis Web Development"
            });
        }

        private List<Page> CreatePageStructure(Page parent, PageType pageType, params int[] levels)
        {
            var list = new List<Page>();
            for (int i = 0; i < levels[0]; i++)
            {
                var page = new Page
                {
                    PageTypeId = pageType.Id,
                    HasChildren = levels.Count() > 1,
                    Name = "Page " + i
                };

                page.SetParent(parent);
                list.Add(page);

                if (levels.Count() > 1) list.AddRange(CreatePageStructure(page, pageType, levels.Skip(1).ToArray()));
            }
            return list;
        }
    }
}
