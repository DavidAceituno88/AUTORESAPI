using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{ 
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;  
        }

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
             //Here i added the include method to show the books related to the autor
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }

        [HttpGet("{id:int}")] //Gets autores by id
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            
            if(autor == null)
            {
                return NotFound();
            }

            return autor;

        }
        
        [HttpGet("{nombre}")] //Gets autores by Name 
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            
            if(autor == null)
            {
                return NotFound();
            }

            return autor;

        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
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
