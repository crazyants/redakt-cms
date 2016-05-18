using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Redakt.Core.Services;
using Redakt.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Redakt.BackOffice.Api.Controllers
{
    [Route("~redakt/api/[controller]")]
    public class ContentController : Controller
    {
        private readonly ISiteService _siteService;

        public ContentController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        [HttpGet("site/{id}")]
        public Site GetSite(string id)
        {
            return _siteService.Get(id);
        }

        [HttpPost("site")]
        public string CreateSite(Site site)
        {
            _siteService.Save(site);
            return site.Id;
        }

        [HttpPut("site/{id}")]
        public void UpdateSite(string id, Site site)
        {
            _siteService.Save(site);
        }
    }
}
