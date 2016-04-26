using Redakt.Data.Repository;

namespace Redakt.Data.Mongo
{
    public class DbSetup: IDbSetup
    {
        private readonly IPageRepository _pageRepository;
        private readonly IConnection _connection;

        public DbSetup(IConnection connection, IPageRepository pageRepository)
        {
            _connection = connection;
            _pageRepository = pageRepository;
        }

        public void Run()
        {
            //if (_addressRepository.AnyAsync(x => x.Id != null).Result) return;

            InitData();
            EnsureIndices();
        }

        public void InitData()
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

        public void EnsureIndices()
        {
            //var addresses = _connection.Database.GetCollection<Address>("Address");
            //addresses.Indexes.CreateOneAsync(Builders<Address>.IndexKeys.Ascending("bag.records.postalCode"));
            //addresses.Indexes.CreateOneAsync(Builders<Address>.IndexKeys.Ascending("bag.records.publicSpaceId"));

            //var publicSpaces = _connection.Database.GetCollection<PublicSpace>("PublicSpace");
            //publicSpaces.Indexes.CreateOneAsync(Builders<PublicSpace>.IndexKeys.Ascending("bag.records.cityId"));

            //var residences = _connection.Database.GetCollection<Residence>("Residence");
            //residences.Indexes.CreateOneAsync(Builders<Residence>.IndexKeys.Ascending("bag.records.primaryAddressId"));
            //residences.Indexes.CreateOneAsync(Builders<Residence>.IndexKeys.Ascending("bag.records.secondaryAddressIds"));
            //residences.Indexes.CreateOneAsync(Builders<Residence>.IndexKeys.Combine("bag.records.primaryAddressId", "bag.records.secondaryAddressIds"));

            //var queue = m_connection.Database.GetCollection<QueueItem>("Queue");
            //queue.Indexes.CreateOneAsync(Builders<QueueItem>.IndexKeys.Descending(x => x.ProcessAtUtc));
            //queue.Indexes.CreateOneAsync(Builders<QueueItem>.IndexKeys.Descending(x => x.StartedAtUtc));
            //queue.Indexes.CreateOneAsync(Builders<QueueItem>.IndexKeys.Descending(x => x.CompletedAtUtc), new CreateIndexOptions { Sparse = true, ExpireAfter = TimeSpan.FromDays(7)});
        }
    }
}
