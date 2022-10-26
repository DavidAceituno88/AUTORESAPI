using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Validations;

namespace WebAPIAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [FirstLetterUpperCaseAttribute]
        [Required]
        public string Titulo { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<AutoresLibros> AutoresLibros { get; set; }
    }
}
