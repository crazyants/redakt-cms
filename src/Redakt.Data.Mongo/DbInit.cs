using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Redakt.Data.Repository;
using Redakt.Model;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Redakt.Data.Mongo
{
    public class DbInit: IDbInit
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IPageTypeRepository _pageTypeRepository;
        private readonly IPageContentRepository _pageContentRepository;
        private readonly IFieldTypeRepository _fieldTypeRepository;
        private readonly IConnection _connection;

        public DbInit(IConnection connection, ISiteRepository siteRepository, IPageRepository pageRepository, IPageTypeRepository pageTypeRepository, IFieldTypeRepository fieldTypeRepository, IPageContentRepository pageContentRepository)
        {
            _connection = connection;
            _siteRepository = siteRepository;
            _pageRepository = pageRepository;
            _pageTypeRepository = pageTypeRepository;
            _pageContentRepository = pageContentRepository;
            _fieldTypeRepository = fieldTypeRepository;
        }

        public void Run()
        {
            RegisterClassMaps();
            EnsureIndices();

            if (!_siteRepository.AnyAsync(x => x.Id != null).Result) PopulateTestData(); ;
        }

        private void RegisterClassMaps()
        {
            BsonClassMap.RegisterClassMap<PersistedEntity> (cm =>
            {
                cm.AutoMap();
                cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
        }

        private void PopulateTestData()
        {
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

            _fieldTypeRepository.SaveAsync(numberFieldType);
            _fieldTypeRepository.SaveAsync(stringFieldType);
            _fieldTypeRepository.SaveAsync(areaFieldType);
            _fieldTypeRepository.SaveAsync(rtfFieldType);

            var contentPageType = new PageType
            {
                Name = "Content Page",
                IconClass = "md md-content-copy"
            };
            contentPageType.Fields.Add(new FieldDefinition { Key = "title", Label = "Titel", FieldTypeId = stringFieldType.Id });
            contentPageType.Fields.Add(new FieldDefinition { Key = "quantity", Label = "Hoeveelheid", FieldTypeId = numberFieldType.Id });
            contentPageType.Fields.Add(new FieldDefinition { Key = "intro", Label = "Introtekst", FieldTypeId = areaFieldType.Id });
            contentPageType.Fields.Add(new FieldDefinition { Key = "body", Label = "Body tekst", FieldTypeId = rtfFieldType.Id });
            _pageTypeRepository.SaveAsync(contentPageType).Wait();

            var homePageType = new PageType
            {
                Name = "Home Page",
                IconClass = "md md-home"
            };
            homePageType.Fields.Add(new FieldDefinition { Key = "intro", Label = "Introtekst", FieldTypeId = areaFieldType.Id });
            homePageType.Fields.Add(new FieldDefinition { Key = "body", Label = "Body tekst", FieldTypeId = rtfFieldType.Id });
            _pageTypeRepository.SaveAsync(homePageType).Wait();

            // Pages
            var homePage1 = new Page
            {
                PageTypeId = homePageType.Id,
                HasChildren = true,
                Name = "Redakt Home"
            };
            var homePage1Content = new PageContent
            {
                PageId = homePage1.Id,
                Culture = "en-US",
                Fields = new Dictionary<string, object>() { { "intro", "Redakt Home intro value" }, { "body", "Redakt Home body value" } }
            };

            var homePage2 = new Page
            {
                PageTypeId = homePageType.Id,
                HasChildren = true,
                Name = "Carvellis Home"
            };
            var homePage2Content = new PageContent
            {
                PageId = homePage2.Id,
                Culture = "en-US",
                Fields = new Dictionary<string, object>() { { "intro", "Carvellis Home intro value" }, { "body", "Carvellis Home body value" } }
            };

            _pageRepository.SaveAsync(homePage1).Wait();
            _pageRepository.SaveAsync(homePage2).Wait();
            _pageContentRepository.SaveAsync(homePage1Content).Wait();
            _pageContentRepository.SaveAsync(homePage2Content).Wait();

            CreatePageStructure(homePage1, contentPageType, 10, 5, 3);
            CreatePageStructure(homePage2, contentPageType, 6, 8);

            // Sites
            _siteRepository.SaveAsync(new Site
            {
                HomePageId = homePage1.Id,
                Name = "Redakt CMS"
            }).Wait();
            _siteRepository.SaveAsync(new Site
            {
                HomePageId = homePage2.Id,
                Name = "Carvellis Web Development"
            }).Wait();
        }

        private void CreatePageStructure(Page parent, PageType pageType, params int[] levels)
        {
            for (int i = 0; i < levels[0]; i++)
            {
                var page = new Page
                {
                    PageTypeId = pageType.Id,
                    HasChildren = levels.Count() > 1,
                    Name = "Page " + i
                };

                page.SetParent(parent);
                _pageRepository.SaveAsync(page).Wait();

                var content = new PageContent
                {
                    PageId = page.Id,
                    Culture = "en-US"
                };
                content.Fields.Add("title", page.Name + " title value");
                content.Fields.Add("quantity", page.Name + " quantity value");
                content.Fields.Add("intro", page.Name + " intro value");
                content.Fields.Add("body", page.Name + " body value");
                _pageContentRepository.SaveAsync(content).Wait();

                if (levels.Count() > 1) CreatePageStructure(page, pageType, levels.Skip(1).ToArray());
            }
        }

        private void EnsureIndices()
        {
            var sites = _connection.Database.GetCollection<Site>("Site");
            //sites.Indexes.CreateOneAsync(Builders<Site>.IndexKeys.Descending(x => x.ProcessAtUtc));

            var pages = _connection.Database.GetCollection<Page>("Page");
            pages.Indexes.CreateOneAsync(Builders<Page>.IndexKeys.Ascending(x => x.AncestorIds));
            pages.Indexes.CreateOneAsync(Builders<Page>.IndexKeys.Ascending(x => x.ParentId));
            pages.Indexes.CreateOneAsync(Builders<Page>.IndexKeys.Ascending(x => x.PageTypeId));

            var content = _connection.Database.GetCollection<PageContent>("PageContent");
            content.Indexes.CreateOneAsync(Builders<PageContent>.IndexKeys.Ascending(x => x.PageId));

            //queue.Indexes.CreateOneAsync(Builders<QueueItem>.IndexKeys.Descending(x => x.CompletedAtUtc), new CreateIndexOptions { Sparse = true, ExpireAfter = TimeSpan.FromDays(7)});
        }
    }
}
