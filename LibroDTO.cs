using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Validations;

namespace WebAPIAutores.DTOs
{
    public class LibroDTO   
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
       // public List<CommentDTO> Comentarios { get; set; }
        public List<AutorDTO> Autores { get; set; }
    
    }
}
