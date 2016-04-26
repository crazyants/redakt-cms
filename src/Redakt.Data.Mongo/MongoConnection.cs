using Microsoft.Extensions.OptionsModel;
using MongoDB.Driver;

namespace Redakt.Data.Mongo
{
    public class MongoConnection: IConnection
    {
        #region [ Variables ]
        private readonly DataSettings _dbSettings;
        
        private readonly MongoClient _client;
        private IMongoDatabase _database;
        #endregion

        #region [ Constructors ]
        public MongoConnection(IOptions<DataSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
            _client = new MongoClient(_dbSettings.MongoConnectionString);
        }
        #endregion

        #region [ Properties ]
        public IMongoDatabase Database => _database ?? (_database = _client.GetDatabase(_dbSettings.MongoDatabase));

        #endregion
    }
}
