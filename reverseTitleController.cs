 [HttpGet("{id:int}/reverse")]
        public async Task<ActionResult<LibroDTO>> GetReverse(int id)
        {   //Here i added the include method to show the autors related to the book.
            var libro = await context.Libros.Include(libroDB => libroDB.Comentarios).FirstOrDefaultAsync(x => x.Id == id);
            //get the title from libro
            char[] titulo = libro.Titulo.ToCharArray();
            //reverse the title string
            Array.Reverse(titulo);
            string reverseTitulo = new string(titulo);
            libro.Titulo = reverseTitulo;
            return mapper.Map<LibroDTO>(libro);
        }
