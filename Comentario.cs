using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIAutores.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int LibroId { get; set; }
        public Libro Libro { get; set; }
    }
}
