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
                Name = "Redakt Home",
                Fields = new List<FieldValue> { new FieldValue { Key = "intro", Value = "Redakt Home intro value" }, new FieldValue { Key = "body", Value = "Redakt Home body value" } }
            };
            var homePage2 = new Page
            {
                PageTypeId = homePageType.Id,
                HasChildren = true,
                Name = "Carvellis Home",
                Fields = new List<FieldValue> { new FieldValue { Key = "intro", Value = "Carvellis Home intro value" }, new FieldValue { Key = "body", Value = "Carvellis Home body value" } }
            };

            _pageRepository.SaveAsync(homePage1).Wait();
            _pageRepository.SaveAsync(homePage2).Wait();
            _pageRepository.SaveAsync(CreatePageStructure(homePage1, contentPageType, 10, 5, 3)).Wait();
            _pageRepository.SaveAsync(CreatePageStructure(homePage2, contentPageType, 6, 8)).Wait();

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
                page.Fields.Add(new FieldValue { Key = "title", Value = page.Name + " title value" });
                page.Fields.Add(new FieldValue { Key = "quantity", Value = page.Name + " quantity value" });
                page.Fields.Add(new FieldValue { Key = "intro", Value = page.Name + " intro value" });
                page.Fields.Add(new FieldValue { Key = "body", Value = page.Name + " body value" });

                page.SetParent(parent);
                list.Add(page);

                if (levels.Count() > 1) list.AddRange(CreatePageStructure(page, pageType, levels.Skip(1).ToArray()));
            }
            return list;
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
