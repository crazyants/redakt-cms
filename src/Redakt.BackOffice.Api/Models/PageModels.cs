using Redakt.Model;
using System.Collections.Generic;
using System.Linq;

namespace Redakt.BackOffice.Api.Models
{
    public class PageTreeItemModel
    {
        public PageTreeItemModel(Page page, string iconClass)
        {
            this.Id = page.Id;
            this.ParentId = page.ParentId;
            this.PageTypeId = page.PageTypeId;
            this.Name = page.Name;
            this.HasChildren = page.HasChildren;
            this.IconClass = iconClass;
        }

        public string Id { get; set; }
        public string ParentId { get; set; }
        public string PageTypeId { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }
        public bool HasChildren { get; set; }
    }

    public class PageUpdateModel
    {
        public string PageTypeId { get; set; }
        public string Name { get; set; }

        public void FillModel(Page page)
        {
            page.PageTypeId = this.PageTypeId;
            page.Name = this.Name;
        }
    }

    public class PageModel: PageUpdateModel
    {
        public PageModel(Page page, IEnumerable<PageContent> pageContent)
        {
            this.Id = page.Id;
            this.ParentId = page.ParentId;
            this.PageTypeId = page.PageTypeId;
            this.Name = page.Name;
            this.Content = pageContent.Select(x => new PageContentModel(x)).ToList();
        }

        public string Id { get; set; }
        public string ParentId { get; set; }
        public List<PageContentModel> Content { get; set; }
    }
}
