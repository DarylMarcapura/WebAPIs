using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiModulo7.Contexts;
using WebApiModulo7.Models;

namespace WebApiModulo7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> usermanager;
        //inyeccion de dependencias
        public UsuariosController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            this.context = context;
            this.usermanager = usermanager;
        }

        [HttpPost("AsignarUsuarioRol")]
        public async Task<ActionResult> AsignarRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await usermanager.FindByIdAsync(editarRolDTO.UserId);
            if (usuario == null)
            {
                return NotFound();
            }
            //agregar nuevo rol , manera clasica
            await usermanager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RolName));
            //agregar rol de la manera jwt
            await usermanager.AddToRoleAsync(usuario, editarRolDTO.RolName);
            return Ok();
        }

        [HttpPost("RemoverUsuarioRol")]
        public async Task<ActionResult> RemoverRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await usermanager.FindByIdAsync(editarRolDTO.UserId);
            if (usuario == null)
            {
                return NotFound();
            }
            //agregar nuevo rol , manera clasica
            await usermanager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RolName));
            //agregar rol de la manera jwt
            await usermanager.RemoveFromRoleAsync(usuario, editarRolDTO.RolName);
            return Ok();
        }
    }
}
