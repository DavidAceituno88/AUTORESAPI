using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;
using System.Linq;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosControllers : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosControllers(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<LibroDTO>>> Get()
        {
        var libro = await context.Libros.Include(libroDB => libroDB.Comentarios).ToListAsync();
        return mapper.Map<List<LibroDTO>>(libro);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {   //Here i added the include method to show the autors related to the book.
            var libro = await context.Libros.Include(libroDB => libroDB.Comentarios).FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<LibroDTO>(libro);
        }

        [HttpGet("random")]
        public async Task<ActionResult<LibroDTO>> GetRandom()
        {
            int total = await context.Libros.CountAsync();
            Random rand = new Random();
            int offset = rand.Next(0,total);
            var result = await context.Libros.Skip(offset).FirstOrDefaultAsync();
            return mapper.Map<LibroDTO>(result);
        }

         [HttpGet("{id:int}/reverse")]
        public async Task<ActionResult<LibroDTO>> GetReverse(int id)
        {   //Here i added the include method to show the autors related to the book.
            var libro = await context.Libros.Include(libroDB => libroDB.Comentarios).FirstOrDefaultAsync(x => x.Id == id);
            //get the title from libro
            char[] titulo = libro.Titulo.ToCharArray();
            //reverse the title string
            Array.Reverse(titulo);
            string reverseTitulo = new string(titulo);
            libro.Titulo = reverseTitulo;
            return mapper.Map<LibroDTO>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreationDTO libroCreationDTO)
        {
            if(libroCreationDTO.AutoresIds == null){
                return BadRequest("You cannot add a book without Authors");
            }
            var autoresIds = await context.Autores.
                            Where(autorDB => libroCreationDTO.AutoresIds.
                            Contains(autorDB.Id)).Select(x => x.Id).ToListAsync();
            
            if (libroCreationDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("One of the Authors does not exists");
            }
            
            var libro = mapper.Map<Libro>(libroCreationDTO);

            if(libro.AutoresLibros != null)
            {
                for(int i = 0 ; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroCreationDTO librocreationDTO)
        {
            var libroDB = await context.Libros.Include(x => x.AutoresLibros).FirstOrDefaultAsync( x => x.Id == id);
            if(libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(librocreationDTO, libroDB);

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
