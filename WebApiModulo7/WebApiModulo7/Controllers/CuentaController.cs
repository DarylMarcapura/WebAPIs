using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiModulo7.Models;

namespace WebApiModulo7.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public CuentaController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration
            )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
        }



        [HttpPost("crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            //crear un usuario a partir de un email y un password
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return BuildToken(model, new List<string>());
            }
            else
            {
                return BadRequest("Username or password invalid");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, false, false);
            if (result.Succeeded)
            {
                var usuario = await _userManager.FindByEmailAsync(userInfo.Email);
                var roles = await _userManager.GetRolesAsync(usuario);
                return BuildToken(userInfo, roles);
            }
            else
            {
                //no se ha logeado correctamente
                ModelState.AddModelError(string.Empty, "invalid login attempt.");
                return BadRequest(ModelState);
            }
        }


        private UserToken BuildToken(UserInfo userInfo, IList<string> roles)
        {
            //informaciones confiables que van a viajar en el token
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("MiValor", "Lo que yo quiera"),
                //identificar de manera unica un token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            //agregar todos los roles al usuario
            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            //llave simetrica de seguridad, nos sirve para poder garantizar la autenticidad
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            //credenciales
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //definir tiempo de expiracion
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
