using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Redakt.Data.Mongo
{
    public class MongoConnection: IConnection
    {
        #region [ Variables ]
        private readonly RedaktSettings _settings;
        
        private readonly MongoClient _client;
        private IMongoDatabase _database;
        #endregion

        #region [ Constructors ]
        public MongoConnection(IOptionsSnapshot<RedaktSettings> settings)
        {
            _settings = settings.Value;
            _client = new MongoClient(_settings.MongoConnectionString);
        }
        #endregion

        #region [ Properties ]
        public IMongoDatabase Database => _database ?? (_database = _client.GetDatabase(_settings.MongoDatabase));

        #endregion
    }
}
