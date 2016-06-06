using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redakt.Data.Repository;
using Redakt.Model;

namespace Redakt.Data.Test.Repository
{
    public class PageRepository : Repository<Page>, IPageRepository
    {
        public Task<IList<Page>> GetChildrenAsync(string pageId)
        {
            return this.FindAsync(x => x.AncestorIds.LastOrDefault() == pageId);
        }

        public Task<IList<Page>> GetDescendantsAsync(string pageId)
        {
            return this.FindAsync(x => x.AncestorIds.Contains(pageId));
        }

        public Task<bool> HasChildrenAsync(string pageId)
        {
            return this.AnyAsync(x => x.ParentId == pageId);
        }

        public async Task SetHasChildrenAsync(string pageId, bool hasChildren)
        {
            var page = await this.GetAsync(pageId);
            page.HasChildren = hasChildren;
            await this.SaveAsync(page);
        }
    }
}
