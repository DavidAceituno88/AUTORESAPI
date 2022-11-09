using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{ 
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            //using automatic mapper now
            var autors = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autors);
        }

        [HttpGet("{id:int}")] //Gets autores by id
        public async Task<ActionResult<AutorDTOWithLibros>> Get(int id)
        {
            var autor = await context.Autores
                                .Include(autorDB => autorDB.AutoresLibros)
                                .ThenInclude(autorlibroDB => autorlibroDB.Libro)
                                .FirstOrDefaultAsync(autorDB => autorDB.Id == id);
            
            if(autor == null)
            {
                return NotFound();
            }

            return mapper.Map<AutorDTOWithLibros>(autor);

        }
        
        [HttpGet("{nombre}")] //Gets autores by Name 
        public async Task<ActionResult<List<AutorDTO>>> Get(string nombre)
        {
            var autores = await context.Autores.Where(autorDB => autorDB.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);

        }

        [HttpPost]
        public async Task<ActionResult> Post(AutorCreationDTO autorCreationDTO)
        {
            //Validation From the DB
            var alreadyExist = await context.Autores.AnyAsync(x => x.Nombre == autorCreationDTO.Nombre);
            if(alreadyExist)
            {
                return BadRequest($"An autor with the name {autorCreationDTO.Nombre} already exists");
            }
            
            var autor = mapper.Map<Autor>(autorCreationDTO);
            
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        
        [HttpPut("{id:int}")] 
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if(autor.Id != id)
            {
                return BadRequest("Id does not match with URL id");
            }
            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Autores.AnyAsync(x => x.Id == id);
            if(!exist)
            {
                return NotFound();
            }
            context.Remove(new Autor() {Id = id});

            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
