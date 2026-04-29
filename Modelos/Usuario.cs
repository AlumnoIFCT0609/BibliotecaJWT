using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaWebApi_JWT.Modelos
{
    public class Usuario
    {
        private int _id;
        private string _nombre;
        private string _dni;
        private DateTime _fechaAlta;

        public Usuario(string nombre, string dni)
        {
            this.Nombre = nombre;
            this.DNI = dni;
            this.FechaAlta = DateTime.Now;
        }
        public Usuario() { }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public string DNI { get => _dni; set => _dni = value; }
        public DateTime FechaAlta { get => _fechaAlta; set => _fechaAlta = value; }
        public int Id { get => _id; set => _id = value; }

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
        public override string ToString()
        {
            return $"Id: {Id} - Nombre: {Nombre} - DNI: {DNI} - Fecha Alta: {FechaAlta}";
        }   
    }
}
