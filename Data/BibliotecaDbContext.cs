using BibliotecaWebApi_JWT.Modelos;
using BibliotecaWebApi_JWT.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace BibliotecaWebApi_JWT.Data
{
    public class BibliotecaDbContext : DbContext
    {

        public DbSet<Libro> Libros => Set<Libro>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Prestamo> Prestamos => Set<Prestamo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Importante si heredas de IdentityDbContext u otro DbContext base

            modelBuilder.Entity<Libro>(e =>
            {
                e.ToTable("Libros");

                // Clave primaria
                e.HasKey(x => x.Id);

                // Propiedades obligatorias y opcionales
                e.Property(x => x.Titulo)
                    .HasMaxLength(200)
                    .IsRequired();

                e.Property(x => x.Autor)
                    .HasMaxLength(150)
                    .IsRequired();
                e.Property(x => x.Categoria)
                    .HasMaxLength(100)
                    .IsRequired();

                e.Property(x => x.Disponible)
                    .IsRequired();

                // Índice único para evitar Libros activos duplicados
                e.HasIndex(x => new { x.Titulo, x.Autor })
                    .IsUnique()
                    .HasDatabaseName("Index_Titulo_Autor");
            });

            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("Usuarios");

                // Clave primaria
                e.HasKey(x => x.Id);

                // Propiedades obligatorias y opcionales
                e.Property(x => x.Nombre)
                    .HasMaxLength(150)
                    .IsRequired();

                e.Property(x => x.DNI)
                    .HasMaxLength(20)
                    .IsRequired();

                e.Property(x => x.FechaAlta)
                    .IsRequired();
                e.HasIndex(x => x.DNI).IsUnique(); // indice unico
            });

            // Configuración de la tabla Prestamos
            modelBuilder.Entity<Prestamo>(e =>
            {
                e.ToTable("Prestamos");

                e.HasKey(x => x.Id);

                e.Property(x => x.FechaPrestamo)
                    .IsRequired();

                e.Property(x => x.FechaDevolucion)
                    .IsRequired();

                e.Property(x => x.FechaDevolucionReal)
                    .IsRequired(false); // Puede ser null

                e.HasOne(x => x.Libro)
                    .WithMany(l => l.Prestamos)
                    .HasForeignKey(x => x.LibroId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Usuario)
                    .WithMany(u => u.Prestamos)
                    .HasForeignKey(x => x.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(x => new { x.LibroId, x.FechaDevolucionReal })
                    .IsUnique();
            });



        }
        public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : base(options)
        {


        }
    }
}
