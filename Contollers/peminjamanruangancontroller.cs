using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarpra.Api.Data;
using Sarpra.Api.Dtos;
using Sarpra.Api.Models;

namespace Sarpra.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeminjamanRuanganController : ControllerBase
    {
        private readonly SarpraDbContext _context;

        public PeminjamanRuanganController(SarpraDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.PeminjamanRuangan
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var entity = await _context.PeminjamanRuangan.FindAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PeminjamanCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NamaPeminjam) ||
                string.IsNullOrWhiteSpace(dto.NamaRuangan) ||
                string.IsNullOrWhiteSpace(dto.Keperluan))
            {
                return BadRequest("Field wajib diisi.");
            }

            if (dto.JamSelesai <= dto.JamMulai)
            {
                return BadRequest("Jam selesai harus lebih besar dari jam mulai.");
            }

            var entity = new PeminjamanRuangan
            {
                NamaPeminjam = dto.NamaPeminjam,
                NamaRuangan = dto.NamaRuangan,
                Keperluan = dto.Keperluan,
                TanggalPeminjaman = dto.TanggalPeminjaman.Date,
                JamMulai = dto.JamMulai,
                JamSelesai = dto.JamSelesai,
                Status = "menunggu"
            };

            _context.PeminjamanRuangan.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PeminjamanCreateDto dto)
        {
            var entity = await _context.PeminjamanRuangan.FindAsync(id);
            if (entity == null) return NotFound();

            if (dto.JamSelesai <= dto.JamMulai)
            {
                return BadRequest("Jam selesai harus lebih besar dari jam mulai.");
            }

            entity.NamaPeminjam = dto.NamaPeminjam;
            entity.NamaRuangan = dto.NamaRuangan;
            entity.Keperluan = dto.Keperluan;
            entity.TanggalPeminjaman = dto.TanggalPeminjaman.Date;
            entity.JamMulai = dto.JamMulai;
            entity.JamSelesai = dto.JamSelesai;

            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entity = await _context.PeminjamanRuangan.FindAsync(id);
            if (entity == null) return NotFound();

            _context.PeminjamanRuangan.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromBody] PeminjamanUpdateStatusDto dto)
        {
            var entity = await _context.PeminjamanRuangan.FindAsync(id);
            if (entity == null) return NotFound();

            var status = (dto.Status ?? "").Trim().ToLowerInvariant();
            var allowed = new[] { "menunggu", "disetujui", "ditolak" };
            if (!allowed.Contains(status))
                return BadRequest("Status harus: menunggu | disetujui | ditolak");

            entity.Status = status;
            await _context.SaveChangesAsync();

            return Ok(entity);
        }
    }
}
