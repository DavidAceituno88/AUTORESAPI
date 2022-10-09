using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIAutores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "The field {0} is required")]
        public string Nombre { get; set; }
        public List<Libro> Libros {get; set;}  
    }
}
