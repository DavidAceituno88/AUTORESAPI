using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreationDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOWithLibros>()
                .ForMember(autorDTO => autorDTO.Libros, options => options.MapFrom(MapAutorDTOLibros));
            
            CreateMap<LibroCreationDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, options => options.MapFrom(MapAuthorBooks));
            CreateMap<Libro, LibroDTO>();
            CreateMap<Libro, LibroDTOWithAutores>()
                .ForMember(libroDTO => libroDTO.Autores, options => options.MapFrom(MapLibroDTOAuthors));
            
            CreateMap<CommentCreationDTO, Comentario>();
            CreateMap<Comentario, CommentDTO>();
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var result = new List<LibroDTO>();

            if(autor.AutoresLibros == null)
            {
                return result;
            }

            foreach (var autorlibro in autor.AutoresLibros)
            {
                result.Add(new LibroDTO()
                {
                    Id = autorlibro.LibroId,
                    Titulo = autorlibro.Libro.Titulo
                });
            }    

            return result;
        }

        private List<AutorDTO> MapLibroDTOAuthors(Libro libro, LibroDTO libroDTO)
        {
            var result = new List<AutorDTO>();
            
            if(libro.AutoresLibros == null) { return result; }

            foreach(var autorlibro in libro.AutoresLibros)
            {
                result.Add(new AutorDTO()
                {
                    Id = autorlibro.AutorId,
                    Nombre = autorlibro.Autor.Nombre
                });
            } 
            return result;
        }
        private List<AutoresLibros> MapAuthorBooks(LibroCreationDTO libroCreationDTO, Libro libro)
        {
            var result = new List<AutoresLibros>();

            if(libroCreationDTO.AutoresIds == null)
            {
                return result;
            }

            foreach(var autorId in libroCreationDTO.AutoresIds)
            {
                result.Add(new AutoresLibros() {AutorId = autorId});
            }

            return result;
        }
    }
}
