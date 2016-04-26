using Redakt.Model;

namespace Redakt.Data.Repository
{
    public interface IPageContentRepository : IAsyncRepository<PageContent>
    {
        //Task UpsertBagAssignments(IEnumerable<Address> addresses);
        //Task UpdateBagPublicSpaces(IEnumerable<Address> addresses);
    }
}
