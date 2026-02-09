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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PeminjamanCreateDto dto)
        {
            var entity = new PeminjamanRuangan
            {
                NamaPeminjam = dto.NamaPeminjam,
                NamaRuangan = dto.NamaRuangan,
                Keperluan = dto.Keperluan,
                TanggalPeminjaman = dto.TanggalPeminjaman.Date,
                JamMulai = dto.JamMulai,
                JamSelesai = dto.JamSelesai,
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "menunggu" : dto.Status.ToLowerInvariant()
            };

            _context.PeminjamanRuangan.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.PeminjamanRuangan
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _context.PeminjamanRuangan
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
                return NotFound(new { message = $"Data dengan id {id} tidak ditemukan." });

            return Ok(data);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PeminjamanUpdateDto dto)
        {
            var entity = await _context.PeminjamanRuangan
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound(new { message = $"Data dengan id {id} tidak ditemukan." });

            entity.NamaPeminjam = dto.NamaPeminjam;
            entity.NamaRuangan = dto.NamaRuangan;
            entity.Keperluan = dto.Keperluan;
            entity.TanggalPeminjaman = dto.TanggalPeminjaman.Date;
            entity.JamMulai = dto.JamMulai;
            entity.JamSelesai = dto.JamSelesai;
            entity.Status = dto.Status.ToLowerInvariant();

            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.PeminjamanRuangan
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound(new { message = $"Data dengan id {id} tidak ditemukan." });

            _context.PeminjamanRuangan.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Data dengan id {id} berhasil dihapus." });
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] PeminjamanUpdateStatusDto dto)
        {
            var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "menunggu", "disetujui", "ditolak"
            };

            if (!allowed.Contains(dto.Status))
                return BadRequest(new { message = "Status tidak valid. Gunakan: menunggu, disetujui, ditolak." });

            var entity = await _context.PeminjamanRuangan.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return NotFound(new { message = $"Data dengan id {id} tidak ditemukan." });

            var statusSebelumnya = entity.Status;
            var statusSekarang = dto.Status.ToLowerInvariant();

            if (string.Equals(statusSebelumnya, statusSekarang, StringComparison.OrdinalIgnoreCase))
            return Ok(entity);

             entity.Status = statusSekarang;

            _context.RiwayatStatusPeminjaman.Add(new RiwayatStatusPeminjaman
{
    PeminjamanId = entity.Id,
    StatusSebelumnya = statusSebelumnya,
    StatusSekarang = statusSekarang,
    DiubahOleh = dto.DiubahOleh,
    Keterangan = dto.Keterangan,
    WaktuPerubahan = DateTime.UtcNow
});



            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpGet("{id:int}/riwayat-status")]
        public async Task<IActionResult> GetRiwayatStatus(int id)
        {
            var exists = await _context.PeminjamanRuangan.AsNoTracking().AnyAsync(x => x.Id == id);
            if (!exists)
                return NotFound(new { message = $"Data dengan id {id} tidak ditemukan." });

            var data = await _context.RiwayatStatusPeminjaman
                .AsNoTracking()
                .Where(x => x.PeminjamanId == id)
                .OrderByDescending(x => x.WaktuPerubahan)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] DateTime? tanggal,
            [FromQuery] string? ruangan,
            [FromQuery] string? status,
            [FromQuery] string? q,
            [FromQuery] string? sort = "id_desc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _context.PeminjamanRuangan.AsNoTracking().AsQueryable();

            if (tanggal.HasValue)
                query = query.Where(x => x.TanggalPeminjaman.Date == tanggal.Value.Date);

            if (!string.IsNullOrWhiteSpace(ruangan))
                query = query.Where(x => x.NamaRuangan.Contains(ruangan));

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(x => x.Status == status.ToLower());

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x =>
                    x.NamaPeminjam.Contains(q) ||
                    x.NamaRuangan.Contains(q) ||
                    x.Keperluan.Contains(q)
                );
            }

            query = sort?.ToLower() switch
            {
                "tanggal_asc" => query.OrderBy(x => x.TanggalPeminjaman).ThenBy(x => x.JamMulai),
                "tanggal_desc" => query.OrderByDescending(x => x.TanggalPeminjaman).ThenByDescending(x => x.JamMulai),
                "id_asc" => query.OrderBy(x => x.Id),
                _ => query.OrderByDescending(x => x.Id)
            };

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                page,
                pageSize,
                total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize),
                items
            });
        }
    }
}
