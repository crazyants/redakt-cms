using Redakt.Model;

namespace Redakt.BackOffice.Api.Models
{
    public class PageTypeListItem
    {
        public PageTypeListItem(PageType pageType)
        {
            this.Id = pageType.Id;
            this.Name = pageType.Name;
            this.IconClass = pageType.IconClass;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }
    }
}
