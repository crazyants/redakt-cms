using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redakt.Core.Services;
using Redakt.Model;

namespace Redakt.Core
{
    public class RedaktContext
    {
        public static RedaktContext Current { get; set; }

        public RedaktContext(IPageService pageService, IPageContentService contentService, IPageTypeService pageTypeService)
        {
            this.PageService = pageService;
            this.PageContentService = contentService;
            this.PageTypeService = pageTypeService;
            this.User = new User();
            this.Site = new Site();
        }

        public IPageService PageService { get; private set; }

        public IPageContentService PageContentService { get; private set; }

        public IPageTypeService PageTypeService { get; private set; }

        public User User { get; private set; }

        public Site Site { get; private set; }
    }
}
