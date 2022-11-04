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

         [HttpPost]
        public async Task<ActionResult> Post(LibroCreationDTO libroCreationDTO)
        {
            var autoresIds = await context.Autores.
                            Where(autorDB => libroCreationDTO.AutoresIds.
                            Contains(autorDB.Id)).Select(x => x.Id).ToListAsync();
            
            if (libroCreationDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("One of the Authors does not exists");
            }
            
            var libro = mapper.Map<Libro>(libroCreationDTO);
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
