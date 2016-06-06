using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redakt.Model;

namespace Redakt.Core.Extensions
{
    public static class PageExtensions
    {
        public static Task<Page> Parent(this Page page)
        {
            return RedaktContext.Current.PageService.Get(page.ParentId);
        }

        public static Task<IList<Page>> Ancestors(this Page page)
        {
            return RedaktContext.Current.PageService.GetAncestors(page);
        }

        public static Task<PageType> PageType(this Page page)
        {
            return RedaktContext.Current.PageTypeService.Get(page.PageTypeId);
        }
    }
}
