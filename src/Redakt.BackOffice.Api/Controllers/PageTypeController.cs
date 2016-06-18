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
    [Route("redakt/api/pagetypes")]
    public class PageTypeController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IPageTypeService _pageTypeService;

        public PageTypeController(IPageService pageService, IPageTypeService pageTypeService)
        {
            _pageService = pageService;
            _pageTypeService = pageTypeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPageType(string id)
        {
            var page = await _pageTypeService.Get(id);
            if (page == null) return NotFound();
            return Ok(page);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetPageTypes()
        {
            var pageTypes = await _pageTypeService.GetAll();
            return Ok(pageTypes.Select(pt => new PageTypeListItem(pt)));
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatePageType(PageType pageType)
        {
            await _pageTypeService.Save(pageType);
            return Ok(pageType.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePageType(string id, PageType pageType)
        {
            await _pageTypeService.Save(pageType);
            return Ok();
        }
    }
}
