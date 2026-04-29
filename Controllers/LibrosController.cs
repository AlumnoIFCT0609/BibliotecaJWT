using BibliotecaWebApi_JWT.Data;
using BibliotecaWebApi_JWT.Dtos.Libros;
using BibliotecaWebApi_JWT.Modelos;
using BibliotecaWebApi_JWT.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BibliotecaWebApi_JWT.Controllers
{
    [ApiController]
    [Route("api/v1/libros")]
    [Produces("application/json", "application/xml")]
    [Authorize]

    public class LibrosController: ControllerBase
    {
        private readonly BibliotecaDbContext _db;
        public LibrosController(BibliotecaDbContext db) => _db = db;
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<LibroReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LibroReadDto>>> GetAll([FromQuery] bool? disponible, [FromQuery] string? categoria, [FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);

            IQueryable<Libro> query = _db.Libros.AsNoTracking()
            .AsNoTracking();
            if (disponible is not null) query = query.Where(l => l.Disponible == disponible.Value);
            if (!string.IsNullOrWhiteSpace(categoria)) query = query.Where(l => l.Categoria == categoria);
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(l => l.Titulo.Contains(q) || l.Autor.Contains(q));
            var items = await query.OrderBy(l => l.Titulo).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
            return Ok(items.Select(x=>x.ToReadDto()).ToList()); // crear metodo en Libro

        
        }

            
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<LibroReadDto>> GetById(int id, CancellationToken ct)
        {
            var libro = await _db.Libros.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, ct);
            return libro is null ? NotFound() : Ok(libro.ToReadDto());
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<ActionResult<LibroReadDto>> Create([FromBody] LibroCreateDto dto, CancellationToken ct)
        {
            var entity = new Libro { Titulo = dto.Titulo, Autor = dto.Autor, Categoria = dto.Categoria, Anio = dto.Anio, Disponible = true };
            _db.Libros.Add(entity);
            await _db.SaveChangesAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToReadDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<LibroReadDto>> Update(int id, [FromBody] LibroUpdateDto dto, CancellationToken ct)
        {
            var entity = await _db.Libros.FirstOrDefaultAsync(l => l.Id == id, ct);
            if (entity is null) return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Titulo) && dto.Titulo != "string")
                entity.Titulo = dto.Titulo;

            if (!string.IsNullOrWhiteSpace(dto.Autor) && dto.Autor != "string")
                entity.Autor = dto.Autor;

            if (!string.IsNullOrWhiteSpace(dto.Categoria) && dto.Categoria != "string")
                entity.Categoria = dto.Categoria;

            if (dto.Anio.HasValue)
                entity.Anio = dto.Anio.Value;

            if (dto.Disponible.HasValue)
                entity.Disponible = dto.Disponible.Value;
            await _db.SaveChangesAsync(ct);
            return Ok(entity.ToReadDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var entity = await _db.Libros.FirstOrDefaultAsync(l => l.Id == id, ct);

            if (entity is null) return NotFound();
            _db.Libros.Remove(entity);
            await _db.SaveChangesAsync(ct);

            return NoContent();

        }



    }
}
