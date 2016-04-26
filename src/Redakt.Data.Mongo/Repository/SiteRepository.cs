using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Redakt.Data.Repository;
using Redakt.Model;

namespace Redakt.Data.Mongo.Repository
{
    public class SiteRepository : Repository<Site>, ISiteRepository
    {
        public SiteRepository(IConnection connection)
            : base(connection)
        {
        }
    }
}
