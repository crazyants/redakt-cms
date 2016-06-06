using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Redakt.Data.Repository;
using Redakt.Model;
using System.Linq;
using System.Reflection;

namespace Redakt.Data.Mongo
{
    public class DbInit: IDbInit
    {
        private readonly IPageRepository _pageRepository;
        private readonly IConnection _connection;

        public DbInit(IConnection connection, IPageRepository pageRepository)
        {
            _connection = connection;
            _pageRepository = pageRepository;
        }

        public void Run()
        {
            //_connection.Database.ListCollections()
            //if (_addressRepository.AnyAsync(x => x.Id != null).Result) return;

            RegisterClassMaps();
            EnsureIndices();
            InitData();
        }

        private void RegisterClassMaps()
        {
            var type = typeof(PersistedEntity);
            var types = type.GetTypeInfo().Assembly.GetTypes().Where(t => type.IsAssignableFrom(t));

            foreach (var t in types)
            {
                BsonClassMap.RegisterClassMap<t.GetType()>(cm =>
                {
                    cm.AutoMap();
                    cm.IdMemberMap.SetRepresentation(BsonType.ObjectId);
                });
            }
        }

        private void InitData()
        {
            //var apiClients = m_connection.Database.GetCollection<ApiClient>("ApiClient");
            //if (apiClients.CountAsync(x => true).Result > 0) return;

            //var client = new ApiClient
            //{
            //    Id = "526ce7f8cc6f100c78c15a7f",
            //    Name = "Palanga Web Client",
            //    RedirectUri = "http://www.palanga.com"
            //};
            //client.Origins.Add("http://www.palanga.com");

            //apiClients.InsertOneAsync(client);
        }

        private void EnsureIndices()
        {
            var sites = _connection.Database.GetCollection<Site>("Site");
            //sites.Indexes.CreateOneAsync(Builders<Site>.IndexKeys.Descending(x => x.ProcessAtUtc));

            var pages = _connection.Database.GetCollection<Page>("Page");
            pages.Indexes.CreateOneAsync(Builders<Page>.IndexKeys.Ascending(x => x.AncestorIds));
            pages.Indexes.CreateOneAsync(Builders<Page>.IndexKeys.Ascending(x => x.PageTypeId));

            var content = _connection.Database.GetCollection<PageContent>("PageContent");
            content.Indexes.CreateOneAsync(Builders<PageContent>.IndexKeys.Ascending(x => x.PageId));

            //queue.Indexes.CreateOneAsync(Builders<QueueItem>.IndexKeys.Descending(x => x.CompletedAtUtc), new CreateIndexOptions { Sparse = true, ExpireAfter = TimeSpan.FromDays(7)});
        }
    }
}
