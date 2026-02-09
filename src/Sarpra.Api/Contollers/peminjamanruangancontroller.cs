using Microsoft.AspNetCore.Mvc;
using Sarpra.Api.Data;
using Sarpra.Api.Models;
using Sarpra.Api.Dtos;

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
        TanggalPeminjaman = dto.TanggalPinjaman,
        JamMulai = dto.JamMulai,
        JamSelesai = dto.JamSelesai,
        Status = string.IsNullOrWhiteSpace(dto.Status) ? "menunggu" : dto.Status
    };

    _context.PeminjamanRuangan.Add(entity);
    await _context.SaveChangesAsync();

    return Ok(entity);
}

    }
}