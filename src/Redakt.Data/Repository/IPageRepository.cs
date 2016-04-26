using System.Collections.Generic;
using System.Threading.Tasks;
using Redakt.Model;

namespace Redakt.Data.Repository
{
    public interface IPageRepository: IAsyncRepository<Page>
    {
        Task<List<Page>> GetChildren(string pageId);
        Task<List<Page>> GetDescendants(string pageId);
        //Task UpsertBagAssignments(IEnumerable<Address> addresses);
        //Task UpdateBagPublicSpaces(IEnumerable<Address> addresses);
    }
}
