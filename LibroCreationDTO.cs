using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Validations;

namespace WebAPIAutores.DTOs
{
    public class LibroCreationDTO
    {
        [Required (ErrorMessage = "The field {0} is required")]
        [StringLength(maximumLength:120, ErrorMessage = "The field {0} must have more than {1} characters")]
        [FirstLetterUpperCaseAttribute]
        public string Titulo { get; set; }
    }
}
