using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaWebApi_JWT.Modelos
{
    public class Prestamo
    {
        private int _id;
        private int _usuarioId;
        private int _libroId;
        private DateTime? _fechaPrestamo;
        private DateTime? _fechaDevolucion;
        private DateTime? _fechaDevolucionReal;
         public Prestamo(int usuarioId, int libroId, DateTime? fechaPrestamo, DateTime? fechaDevolucionReal)
        {
            this.UsuarioId = usuarioId;
            this.LibroId = libroId;
            this.FechaPrestamo = fechaPrestamo;
            this.FechaDevolucion = fechaPrestamo?.AddDays(15);
            this.FechaDevolucionReal = fechaDevolucionReal;
        }

        public Prestamo() { }
        public Usuario Usuario { get; set; }
        public Libro Libro { get; set; }
        public int Id { get => _id; set => _id = value; }
     
        public DateTime? FechaPrestamo { get => _fechaPrestamo; set => _fechaPrestamo = value; }
        public DateTime? FechaDevolucion { get => _fechaDevolucion; set => _fechaDevolucion = value; }
        public DateTime? FechaDevolucionReal { get => _fechaDevolucionReal; set => _fechaDevolucionReal = value; }
        public int LibroId { get => _libroId; set => _libroId = value; }
        public int UsuarioId { get => _usuarioId; set => _usuarioId = value; }

        public bool EstaVencido()
        {
            if (FechaDevolucionReal.HasValue && FechaDevolucion.HasValue)
            {
                return FechaDevolucionReal.Value > FechaDevolucion.Value;
                // se ha devuelto tarde
            }
            if (!FechaDevolucionReal.HasValue)
            {
                return DateTime.Now > FechaDevolucion;
                // aún no se ha devuelto, pero la fecha de devolución ya pasó
            }
            return false;
        }

        public override string ToString()
        {
            return $"Prestamo [Id={Id}, IdUsuario={UsuarioId}, IdLibro={LibroId}, FechaPrestamo={FechaPrestamo?.ToString("yyyy-MM-dd")}, FechaDevolucion={FechaDevolucion?.ToString("yyyy-MM-dd")}, FechaDevolucionReal={FechaDevolucionReal?.ToString("yyyy-MM-dd")}]";    
        }
    }
}
