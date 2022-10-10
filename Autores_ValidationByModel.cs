using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Validations;

namespace WebAPIAutores.Entidades
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }
        
        [Required (ErrorMessage = "The field {0} is required")]
        [StringLength(maximumLength:5, ErrorMessage = "The field {0} must have more than {1} characters")]
        public string Nombre { get; set; }
        public List<Libro> Libros {get; set;}  
        
        public IEnumerable<ValidationResult> Validate(ValidationContext  validationContext)
        {
            if(!string.IsNullOrEmpty(Nombre))
            {
                var firstLetter = Nombre[0].ToString();
                
                if(firstLetter != firstLetter.ToUpper())
                {
                      yield return new ValidationResult("The first letter must be UpperCase",
                      new string[] {nameof Nombre});
                }
            }
        }
    }
}
