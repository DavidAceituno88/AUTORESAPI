using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIAutores.DTOs
{
    public class LibroDTOWithAutores : LibroDTO
    {
        public List<AutorDTO> Autores { get; set; }
    }
}
