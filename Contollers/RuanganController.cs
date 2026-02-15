using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarpra.Api.Data;
using Sarpra.Api.Dtos;
using Sarpra.Api.Models;

namespace Sarpra.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RuanganController : ControllerBase
    {
        private readonly SarpraDbContext _context;

        public RuanganController(SarpraDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Ruangan
                .OrderBy(x => x.Id)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("tersedia")]
        public async Task<IActionResult> GetTersedia()
        {
            var data = await _context.Ruangan
                .Where(x => x.Status.ToLower() == "tersedia")
                .OrderBy(x => x.Id)
                .ToListAsync();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RuanganCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NamaRuangan) || string.IsNullOrWhiteSpace(dto.Keterangan))
                return BadRequest("NamaRuangan dan Keterangan wajib diisi.");

            var status = (dto.Status ?? "tersedia").Trim().ToLowerInvariant();
            var allowed = new[] { "tersedia", "dipinjam", "maintenance" };
            if (!allowed.Contains(status))
                return BadRequest("Status harus: tersedia | dipinjam | maintenance");

            var entity = new Ruangan
            {
                NamaRuangan = dto.NamaRuangan.Trim(),
                Keterangan = dto.Keterangan.Trim(),
                Status = status,
                CreatedAt = DateTime.Now
            };

            _context.Ruangan.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromBody] RuanganUpdateStatusDto dto)
        {
            var entity = await _context.Ruangan.FindAsync(id);
            if (entity == null) return NotFound();

            var status = (dto.Status ?? "").Trim().ToLowerInvariant();
            var allowed = new[] { "tersedia", "dipinjam", "maintenance" };
            if (!allowed.Contains(status))
                return BadRequest("Status harus: tersedia | dipinjam | maintenance");

            entity.Status = status;
            entity.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(entity);
        }
    }
}
