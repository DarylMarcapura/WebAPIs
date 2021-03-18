using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiModulo7.Services;

namespace WebApiModulo7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //Añadir autenticacion de , solo usuarios con rol admin pueden utilizar
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    //Habilitar cors para todo el controlador
    //[EnableCors("PermitirApiRequest")]
    public class ValuesController : ControllerBase
    {
        private readonly IDataProtector _protector;
        private readonly HashService _hashService;

        public ValuesController(IDataProtectionProvider protectionProvider, HashService hashService)
        {
            //protector encripa y desencripta la data
            _protector = protectionProvider.CreateProtector("valor_unico_y_quizas_secreto");
            //servicio de hash inyectado
            _hashService = hashService;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("hash")]
        public ActionResult GetHash()
        {
            string textoPlano = "Felipe Gavilán";
            //utilizar servicio para utilizar un hash
            var hashResult1 = _hashService.Hash(textoPlano).Hash;
            var hashResult2 = _hashService.Hash(textoPlano).Hash;
            return Ok(new { textoPlano, hashResult1, hashResult2 });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            string textoPlano = "Felipe Gavilán";
            //encriptar texto
            string textoCifrado = _protector.Protect(textoPlano);
            //desencriptar texto
            string textoDesencriptado = _protector.Unprotect(textoCifrado);
            return Ok(new { textoPlano, textoCifrado, textoDesencriptado });
        }

        private void EjemploDeEncriptacionLimitadaPorTiempo()
        {
            //indicar que va a estar encriptado por tiempo
            var protectorLimitadoPorTiempo = _protector.ToTimeLimitedDataProtector();
            string textoPlano = "Felipe Gavilán";
            //encriptar y asignar un tiempo de encriptacion
            string textoCifrado = protectorLimitadoPorTiempo.Protect(textoPlano, TimeSpan.FromSeconds(5));
            string textoDesencriptado = protectorLimitadoPorTiempo.Unprotect(textoCifrado);
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
