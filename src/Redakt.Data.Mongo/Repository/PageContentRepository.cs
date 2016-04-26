using Redakt.Data.Repository;
using Redakt.Model;

namespace Redakt.Data.Mongo.Repository
{
    public class PageContentRepository : Repository<PageContent>, IPageContentRepository
    {
        public PageContentRepository(IConnection connection)
            : base(connection)
        {
        }
    }
}
