using Microsoft.AspNetCore.Mvc;
using Redakt.Core.Services;
using Redakt.Model;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Redakt.BackOffice.Api.Controllers
{
    [Route("redakt/api/content")]
    public class ContentController : Controller
    {
        private readonly IPageContentService _pageContentService;

        public ContentController(IPageContentService pageContentService)
        {
            _pageContentService = pageContentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContent(string id)
        {
            var content = await _pageContentService.Get(id);
            if (content == null) return NotFound();
            return Ok(content);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateContent(PageContent content)
        {
            await _pageContentService.Save(content);
            return Ok(content.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContent(string id, PageContent content)
        {
            await _pageContentService.Save(content);
            return Ok();
        }
    }
}
