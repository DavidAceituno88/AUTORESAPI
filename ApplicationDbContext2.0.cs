using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entidades;

namespace WebAPIAutores
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AutoresLibros>().HasKey(al => new {al.AutorId , al.LibroId});
        }

        public DbSet<Autor> Autores {get; set;}
        public DbSet<Libro> Libros {get; set;}
        public DbSet<Comentario> Comentarios { get; set;}
        public DbSet<AutoresLibros> AutoresLibros {get; set;}
    }
}
