using Microsoft.EntityFrameworkCore;
using MiPrimerWebApiM3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApiM3.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        //manera estandar de configurar un proyecto entity core
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //amarrando clases con tablas
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
    }
}
