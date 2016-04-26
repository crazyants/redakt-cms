using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Redakt.Data.Repository;
using Redakt.Model;

namespace Redakt.Data.Mongo.Repository
{
    public class PageRepository : Repository<Page>, IPageRepository
    {
        public PageRepository(IConnection connection)
            : base(connection)
        {
        }

        public Task<List<Page>> GetChildren(string pageId)
        {
            return this.Collection.Find(x => x.AncestorIds.LastOrDefault() == pageId).Limit(null).ToListAsync();
        }

        public Task<List<Page>> GetDescendants(string pageId)
        {
            return this.Collection.Find(x => x.AncestorIds.Contains(pageId)).Limit(null).ToListAsync();
        }
    }
}
