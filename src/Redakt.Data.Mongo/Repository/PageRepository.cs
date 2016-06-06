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

        public Task<IList<Page>> GetChildren(string pageId)
        {
            return this.FindAsync(x => x.AncestorIds.LastOrDefault() == pageId);
        }

        public Task<IList<Page>> GetDescendants(string pageId)
        {
            return this.FindAsync(x => x.AncestorIds.Contains(pageId));
        }
    }
}
