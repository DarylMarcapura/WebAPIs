using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiModulo7.Models;

namespace WebApiModulo7.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Creando roles, https://www.guidgenerator.com/
            var roleAdmin = new IdentityRole()
            {
                Id = "",
                Name = "admin",
                NormalizedName = "admin"
            };
            base.OnModelCreating(builder);
        }
    }
}
