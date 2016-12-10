using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Redakt.BackOffice.Api.Models;
using Redakt.Core.Services;
using Redakt.Model;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Redakt.BackOffice.Api.Controllers
{
    [Route("redakt/api/pages")]
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IPageTypeService _pageTypeService;

        public PageController(IPageService pageService, IPageTypeService pageTypeService)
        {
            _pageService = pageService;
            _pageTypeService = pageTypeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPage(string id)
        {
            var page = await _pageService.Get(id);
            if (page == null) return NotFound();

            return Ok(page);
        }

        [HttpGet("{id}/treeitem")]
        public async Task<IActionResult> GetPageTreeItem(string id)
        {
            var page = await _pageService.Get(id);
            if (page == null) return NotFound();
            var pageType = await _pageTypeService.Get(page.PageTypeId);

            return Ok(new PageTreeItemModel(page, pageType.IconClass));
        }

        [HttpGet("{id}/children/treeitem")]
        public async Task<IActionResult> GetChildrenTreeItem(string id)
        {
            var pages = await _pageService.GetChildren(id);
            var pageTypes = new List<PageType>();
            foreach (var page in pages)
            {
                if (!pageTypes.Any(pt => pt.Id == page.PageTypeId)) pageTypes.Add(await _pageTypeService.Get(page.PageTypeId));
            }
            return Ok(pages.Select(p => new PageTreeItemModel(p, pageTypes.First(pt => pt.Id == p.PageTypeId).IconClass)));
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatePage(Page page)
        {
            await _pageService.Save(page);
            return Ok(page.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePage(string id, Page page)
        {
            await _pageService.Save(page);
            return Ok();
        }
    }
}
