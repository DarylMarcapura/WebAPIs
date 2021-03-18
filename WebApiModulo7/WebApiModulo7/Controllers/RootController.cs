using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiModulo7.Models;

namespace WebApiModulo7.Controllers
{
    [ApiController]
    [Route("api")]
    public class RootController: ControllerBase
    {
        [HttpGet("GetRoot")]
        public ActionResult<IEnumerable<Enlace>> Get()
        {
            List<Enlace> enlaces = new List<Enlace>();
            //generacion de urls
            enlaces.Add(new Enlace(href: Url.Link("GetRoot", new { }), rel: "self", metodo: "GET"));
            return enlaces;
        }
    }
}
