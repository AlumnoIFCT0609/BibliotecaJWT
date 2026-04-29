using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaWebApi_JWT.Data;
using BibliotecaWebApi_JWT.Dtos.Usuarios;
using BibliotecaWebApi_JWT.Modelos;
using BibliotecaWebApi_JWT.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BibliotecaWebApi_JWT.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    [Produces("application/json", "application/xml")]
    [Authorize]
    public class UsuariosController:ControllerBase
    {
        private readonly BibliotecaDbContext _db;
        public UsuariosController(BibliotecaDbContext db) => _db = db;

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<IEnumerable<UsuarioReadDto>>> GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);
            IQueryable<Usuario> query = _db.Usuarios.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(u => u.Nombre.Contains(q) || u.DNI.Contains(q));

            var items = await query.OrderBy(u => u.Nombre).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
            return Ok(items.Select(x => x.ToReadDto()).ToList());
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<UsuarioReadDto>> GetById(int id, CancellationToken ct)
        { 
            var user = await _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
            return user is null ? NotFound() : Ok(user.ToReadDto());


        }
        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<UsuarioReadDto>> Create([FromBody] UsuarioCreateDto dto, CancellationToken ct)
        {
            bool exists = await _db.Usuarios.AnyAsync(u => u.DNI == dto.DNI, ct);
            if (exists)
            {
                return Conflict(new ProblemDetails { Title = "Conflicto", Detail = "Ya existe un usuario con ese DNI.", Status = 409 });
            }
                var entity = new Usuario { Nombre = dto.Nombre, DNI = dto.DNI, FechaAlta = DateTime.Now };
            _db.Usuarios.Add(entity);
            await _db.SaveChangesAsync(ct);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToReadDto());
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<UsuarioReadDto>> Update(int id, [FromBody] UsuarioUpdateDto dto, CancellationToken ct)
        {
            var entity = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == id, ct);
            if (entity is null) return NotFound();
            if (!string.IsNullOrWhiteSpace(dto.Nombre) && dto.Nombre != "string")
                entity.Nombre = dto.Nombre;
            await _db.SaveChangesAsync(ct);
            return Ok(entity.ToReadDto());
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var entity = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == id, ct);
            if (entity is null) return NotFound();
            _db.Usuarios.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }


    }
}
