using Redakt.Model;

namespace Redakt.BackOffice.Api.Models
{
    public class PageTreeItem
    {
        public PageTreeItem(Page page, string iconClass)
        {
            this.Id = page.Id;
            this.ParentId = page.ParentId;
            this.Name = page.Name;
            this.HasChildren = page.HasChildren;
            this.IconClass = iconClass;
        }

        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }
        public bool HasChildren { get; set; }
    }
}
