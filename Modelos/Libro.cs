using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaWebApi_JWT.Modelos
{
   
    public class Libro
    {
        private int _id;
        private string _titulo;
        private string _autor;
        private string _categoria;
        private int _anio;
        private bool _disponible;
       

        public Libro(string titulo, string autor, string categoria)
        {
            this.Titulo = titulo;
            this.Autor = autor;
            this.Categoria = categoria;
            this.Disponible = true;
        }
        public Libro() { }

        public string Titulo { get => _titulo; set => _titulo = value; }
        public string Autor { get => _autor; set => _autor = value; }
        public string Categoria { get => _categoria; set => _categoria = value; }
        public bool Disponible { get => _disponible; set => _disponible = value; }
        public int Id { get => _id; set => _id = value; }
        public int Anio { get => _anio; set => _anio = value; }

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
        public override string ToString()
        {
            return $"Id: {Id} - Titulo: {Titulo} - Autor: {Autor} - Categoria: {Categoria} - Año: {Anio} - Disponible: {Disponible}";
        }
    }

}
