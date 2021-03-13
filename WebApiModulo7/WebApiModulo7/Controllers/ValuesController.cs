using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiModulo7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //Añadir autenticacion de , solo usuarios con rol admin pueden utilizar
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class ValuesController : ControllerBase
    {
        private readonly IDataProtectionProvider protectionProvider;

        public ValuesController(IDataProtectionProvider protectionProvider, HashService hashService )
        {
            this.protectionProvider = protectionProvider;
        }
    }
}
