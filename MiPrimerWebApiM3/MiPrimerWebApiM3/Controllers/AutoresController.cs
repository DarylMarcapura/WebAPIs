using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPut("{id}")] //actualizaciones completas del recurso
        public async Task<ActionResult> Put(int id, [FromBody] AutorCreacionDTO autorActualizacion)
        {
            var autor = mapper.Map<Autor>(autorActualizacion);
            autor.Id = id;
            context.Entry(autor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Autor> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var autorBD = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autorBD == null)
            {
                return NotFound();
            }
            patchDocument.ApplyTo(autorBD, ModelState);
            var isValid = TryValidateModel(autorBD);
            if (!isValid)
            {
                return BadRequest();
            }
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Autor>> Delete(int id)
        {
            //busca solo el id de un recurso en la base de datos
            var autorid = await context.Autores.Select(x => x.Id).FirstOrDefaultAsync(x => x == id);
            if (autorid == default(int))
            {
                return NotFound();
            }
            context.Autores.Remove(new Autor { Id = autorid });
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
