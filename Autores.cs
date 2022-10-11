using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Validations;

namespace WebAPIAutores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        
        [Required (ErrorMessage = "The field {0} is required")]
        [StringLength(maximumLength:120, ErrorMessage = "The field {0} must have more than {1} characters")]
        [FirstLetterUpperCaseAttribute] //Validation created in the validations folder
        public string Nombre { get; set; }
        public List<Libro> Libros {get; set;}  
    }
}
