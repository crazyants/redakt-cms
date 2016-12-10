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
        private readonly IPageContentService _pageContentService;

        public PageController(IPageService pageService, IPageTypeService pageTypeService, IPageContentService pageContentService)
        {
            _pageService = pageService;
            _pageTypeService = pageTypeService;
            _pageContentService = pageContentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPage(string id)
        {
            var page = await _pageService.Get(id);
            if (page == null) return NotFound();

            var content = await _pageContentService.GetForPage(page.Id);

            return Ok(new PageModel(page, content));
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

        [HttpPost("{parentId}/children")]
        public async Task<IActionResult> CreatePage(string parentId, [FromBody]PageUpdateModel model)
        {
            var parent = await _pageService.Get(parentId);
            if (parent == null) return BadRequest();

            var page = new Page();
            model.FillModel(page);
            page.SetParent(parent);

            await _pageService.Save(page);
            return Ok(page.Id);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateRootPage([FromBody]PageUpdateModel model)
        {
            var page = new Page();
            model.FillModel(page);

            await _pageService.Save(page);
            return Ok(page.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePage(string id, [FromBody]PageUpdateModel model)
        {
            var page = await _pageService.Get(id);
            if (page == null) return BadRequest();

            model.FillModel(page);

            await _pageService.Save(page);
            return Ok();
        }
    }
}
