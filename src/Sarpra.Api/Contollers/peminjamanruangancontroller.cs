using Microsoft.AspNetCore.Mvc;
using Sarpra.Api.Data;
using Sarpra.Api.Models;
using Sarpra.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Sarpra.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeminjamanRuanganController : ControllerBase
    {
        private readonly SarpraDbContext _context;
        private object id;

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
        TanggalPeminjaman = dto.TanggalPinjaman,
        JamMulai = dto.JamMulai,
        JamSelesai = dto.JamSelesai,
        Status = string.IsNullOrWhiteSpace(dto.Status) ? "menunggu" : dto.Status
    };

    _context.PeminjamanRuangan.Add(entity);
    await _context.SaveChangesAsync();

    return Ok(entity);
}
[HttpGet("{id:int}")]
public async Task<IActionResult> GetAll()
{
    var data = await _context.PeminjamanRuangan
        .AsNoTracking()
        .OrderByDescending(x => x.Id)
        .ToListAsync();

    if (data == null)
    {
        return NotFound(new {message = $"Data dengan id {id} tidak ditemukan"});
    }

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
    entity.Status = dto.Status;

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




    }
}