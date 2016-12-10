using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redakt.Data.Repository;
using Redakt.Model;

namespace Redakt.Data.Mongo.Repository
{
    public class PageTypeRepository : Repository<PageType>, IPageTypeRepository
    {
        public PageTypeRepository(IConnection connection)
            : base(connection)
        {
        }
    }
}
