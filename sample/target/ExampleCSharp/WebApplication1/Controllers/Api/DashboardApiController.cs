using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Controllers.Filters;
using WebApplication1.Repositories.Entities;
using WebApplication1.Services;
using WebApplication1.Services.Models;
using WebApplication1.Repositories.Models;

namespace WebApplication1.Controllers.Api
{
    [RequestFilter]
    [ApiController]
    [Produces("application/json")]
    [Route("api/Dashboard")]
    public class DashboardApiController : ControllerBase
    {
        public DashboardApiController(DashboardService service)
        {
            this.service = service;
        }

        private DashboardService service { get; set; }

        [HttpGet("")]
        public List<DashboardItem> List()
        {
            return this.service.List();
        }
    }
}

