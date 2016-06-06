using System.Linq;
using System.Threading.Tasks;
using Redakt.BackOffice.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Redakt.Core.Services;
using Redakt.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Redakt.BackOffice.Api.Controllers
{
    [Route("redakt/api/sites")]
    public class SiteController : Controller
    {
        private readonly ISiteService _siteService;

        public SiteController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSite(string id)
        {
            var site = await _siteService.Get(id);
            if (site == null) return NotFound();
            return Ok(site);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetSites()
        {
            var sites = await _siteService.GetAll();
            return Ok(sites.Select(s => new SiteListItem(s)));
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateSite(Site site)
        {
            await _siteService.Save(site);
            return Ok(site.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSite(string id, Site site)
        {
            await _siteService.Save(site);
            return Ok();
        }
    }
}
