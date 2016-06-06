using System.Collections.Generic;
using System.Threading.Tasks;
using Redakt.Model;

namespace Redakt.Data.Repository
{
    public interface IPageRepository: IAsyncRepository<Page>
    {
        Task<IList<Page>> GetChildrenAsync(string pageId);
        Task<IList<Page>> GetDescendantsAsync(string pageId);
        Task<bool> HasChildrenAsync(string pageId);
        Task SetHasChildrenAsync(string pageId, bool hasChildren);
    }
}
