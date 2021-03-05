using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiPrimerWebApiM3.Contexts;
using MiPrimerWebApiM3.Entities;
using MiPrimerWebApiM3.Helpers;
using MiPrimerWebApiM3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApiM3.Controllers
{
    [Route("api/[controller]")] //ruteo que se corresponde con el controlador api/Autor[controller]
    [ApiController]
    public class AutoresController : ControllerBase // para indicar que es un controlador se debe de heredar ControllerBase
    {
        //inyeccion de dependencias
        private readonly ApplicationDbContext context;
        private readonly ILogger<AutoresController> logger;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, ILogger<AutoresController> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        //multiples endpoins 
        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listado")]
        [HttpGet]
        //agregar filtro personalizado
        [ServiceFilter(typeof(MiFiltrodeAccion))]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get()
        {
            logger.LogInformation("Obteniendo los autores");
            var autores = await context.Autores.Include(x => x.Libros).ToListAsync();
            var autoresDTO = mapper.Map<List<AutorDTO>>(autores);
            return autoresDTO;
        }

        //combinacion de ruta mas un prefijo
        [HttpGet("Primer")]
        public ActionResult<Autor> GetPrimer()
        {
            return context.Autores.FirstOrDefault();
        }

        ////simbolo de interrogacion para indicar que son parametros opcionales
        //[HttpGet("{id}/{param2?}", Name = "ObtenerAutor")]
        ////valor por defecto con el simbolo =
        ////[HttpGet("{id}/{param2=prueba}", Name = "ObtenerAutor")]
        //public async Task<ActionResult<Autor>> Get(int id, string param2)
        //{
        //    //funcion asincrona, el await espera la operacion
        //    var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);
        //    if (autor == null)
        //    {
        //        //cuando no encuentra un autor retorna un 404
        //        return NotFound();
        //    }
        //    return autor;
        //}

        [HttpGet("{id}", Name = "ObtenerAutor")]
        // se debe de proveer el parametro 2 [BindRequired]
        public async Task<ActionResult<AutorDTO>> Get(int id, [BindRequired] string param2)
        {
            //funcion asincrona, el await espera la operacion
            var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
            {
                logger.LogWarning($"El autor del id: {id} no ha sido encontrado");
                //cuando no encuentra un autor retorna un 404
                return NotFound();
            }
            //mapeando un dto
            var autorDTO = mapper.Map<AutorDTO>(autor);
            return autorDTO;

        }

        [HttpPost(Name = "")]
        //frombody indica que debe de buscar en el cuerpo de la peticion http el autor
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacion)
        {
            var autor = mapper.Map<Autor>(autorCreacion);
            context.Autores.Add(autor);
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);
            //devolvemos la ruta 
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autorDTO);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Autor value)
        {
            //id de la url y el recurso sean iguales
            if (id != value.Id)
            {
                return BadRequest();
            }
            //informa que se ha modificado el valor
            context.Entry(value).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Autor> Delete(int id)
        {
            var autor = context.Autores.FirstOrDefault(x => x.Id == id);
            if (autor == null)
            {
                return NotFound();
            }
            context.Autores.Remove(autor);
            context.SaveChanges();
            return autor;
        }

    }
}
