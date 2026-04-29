using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaWebApi_JWT.Data;
using BibliotecaWebApi_JWT.Dtos.Prestamos;
using BibliotecaWebApi_JWT.Modelos;
using BibliotecaWebApi_JWT.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BibliotecaWebApi_JWT.Controllers
{
    [ApiController]
    [Route("api/v1/prestamos")]
    [Produces("application/json", "application/xml")]
    [Authorize]
    public class PrestamosController: ControllerBase
    {
        private readonly BibliotecaDbContext _db;
        public PrestamosController(BibliotecaDbContext db) => _db = db;

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<IEnumerable<PrestamoReadDto>>> GetAll([FromQuery] bool? activos, [FromQuery] bool? vencidos, CancellationToken ct = default)
        {
            IQueryable<Prestamo> query = _db.Prestamos.Include(p => p.Libro).Include(p => p.Usuario).AsNoTracking();
            if (activos is not null) query = activos.Value ? query.Where(p => p.FechaDevolucionReal == null) : query.Where(p => p.FechaDevolucionReal != null);
            if (vencidos is not null && vencidos.Value) query = query.Where(p => p.FechaDevolucionReal == null && DateTime.Now > p.FechaDevolucion);
            var items = await query.OrderByDescending(p => p.FechaPrestamo).ToListAsync(ct);
            return Ok(items.Select(p => p.ToReadDto()).ToList());


        }
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<PrestamoReadDto>> GetById(int id, CancellationToken ct)
        {
            var p = await _db.Prestamos.Include(x => x.Libro).Include(x => x.Usuario).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            return p is null ? NotFound() : Ok(p.ToReadDto());

        }
        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<PrestamoReadDto>> Create([FromBody] PrestamoCreateDto dto, CancellationToken ct)
        {
            var libro = await _db.Libros.FirstOrDefaultAsync(l => l.Id == dto.LibroId, ct);
            if (libro is null) return NotFound(new ProblemDetails { Title = "No encontrado", Detail = "Libro no encontrado", Status = 404 });
            if (!libro.Disponible) return Conflict(new ProblemDetails { Title = "Conflicto", Detail = "El libro no está disponible (ya prestado).", Status = 409 });


            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == dto.UsuarioId, ct);
            if (usuario is null) return NotFound(new ProblemDetails { Title = "No encontrado", Detail = "Usuario no encontrado", Status = 404 });
            int dias = Math.Clamp(dto.DiasPrestamo, 1, 60);
            var ahora = DateTime.Now;
            var prestamo = new Prestamo { LibroId = libro.Id, UsuarioId = usuario.Id, FechaPrestamo = ahora, FechaDevolucion = ahora.AddDays(dias), FechaDevolucionReal = null };
            libro.Disponible = false;
            _db.Prestamos.Add(prestamo);
            await _db.SaveChangesAsync(ct);
            var creado = await _db.Prestamos.Include(p => p.Libro).Include(p => p.Usuario).AsNoTracking().FirstAsync(p => p.Id == prestamo.Id, ct);
            return CreatedAtAction(nameof(GetById), new { id = prestamo.Id }, creado.ToReadDto());

        }

        [HttpPatch("{id:int}/devolver")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<PrestamoReadDto>> Devolver(int id, [FromBody] PrestamoDevolverDto dto, CancellationToken ct)
        {
            var prestamo = await _db.Prestamos.Include(p => p.Libro).Include(p => p.Usuario).FirstOrDefaultAsync(p => p.Id == id, ct);
            if (prestamo is null) return NotFound();
            if (prestamo.FechaDevolucionReal != null) return Conflict(new ProblemDetails { Title = "Conflicto", Detail = "El préstamo ya está devuelto.", Status = 409 });
            prestamo.FechaDevolucionReal = DateTime.Now;
            prestamo.Libro.Disponible = true;
            await _db.SaveChangesAsync(ct);
            return Ok(prestamo.ToReadDto());
        }

    }
}
