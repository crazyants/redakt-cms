using Redakt.Model;

namespace Redakt.BackOffice.Api.Models
{
    public class SiteListItemModel
    {
        public SiteListItemModel(Site site)
        {
            this.Id = site.Id;
            this.Name = site.Name;
            this.HomePageId = site.HomePageId;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string HomePageId { get; set; }
    }
}
