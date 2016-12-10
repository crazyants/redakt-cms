using Redakt.Model;
using System.Collections.Generic;

namespace Redakt.BackOffice.Api.Models
{
    public class PageContentUpdateModel
    {
        public string Culture { get; set; }
        public Dictionary<string, object> Fields { get; set; }

        public void FillModel(PageContent pageContent)
        {
            pageContent.Culture = this.Culture;
            pageContent.Fields = this.Fields;
        }
    }

    public class PageContentModel : PageContentUpdateModel
    {
        public PageContentModel(PageContent pageContent)
        {
            this.Id = pageContent.Id;
            this.PageId = pageContent.PageId;
            this.Culture = pageContent.Culture;
            this.Fields = pageContent.Fields;
        }

        public string Id { get; set; }
        public string PageId { get; set; }
    }
}
