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
    [Route("api/libros/{libroId:int}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public CommentsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get(int libroId)
        {
            var exist = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);
            if(!exist)
            {
                return BadRequest();
            } 
            var comment = await context.Comentarios
                .Where(commentDB => commentDB.LibroId == libroId).ToListAsync();

            return mapper.Map<List<CommentDTO>>(comment);
        }

        [HttpGet("{id:int}", Name = "GetComment")]
        public async Task<ActionResult<CommentDTO>> GetById(int id )
        {
            var comment = await context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if(comment == null)
            {
                return BadRequest($"There is no comment with id: {id}");
            }

            return mapper.Map<CommentDTO>(comment);

        }

        [HttpPost]
        public async Task<ActionResult> Post(int libroId, CommentCreationDTO commentCreationDTO)
        {
            var exist = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);
            if(!exist)
            {
                return BadRequest();
            }

            var comment = mapper.Map<Comentario>(commentCreationDTO);
            comment.LibroId = libroId;
            context.Add(comment);
            await context.SaveChangesAsync();
            var commentDTO = mapper.Map<CommentDTO>(comment);
            return CreatedAtRoute("GetComment", new {id = comment.Id, libroId = libroId}, commentDTO );
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, CommentCreationDTO commentCreationDTO )
        {
            var bookExists = await context.Libros.AnyAsync(libroDb => libroDb.Id == libroId);
            
            if(!bookExists)
            {
                return NotFound();
            }

            var commentExists = await context.Comentarios.AnyAsync(commentDb => commentDb.Id == id);
            
            if(!commentExists)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comentario>(commentCreationDTO);
            comment.Id = id;
            comment.LibroId = libroId;
            context.Update(comment);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
