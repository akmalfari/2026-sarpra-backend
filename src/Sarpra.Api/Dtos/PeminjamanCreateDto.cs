using System.ComponentModel.DataAnnotations;

namespace Sarpra.Api.Dtos
{
    public class PeminjamanCreateDto
    {
        [Required]
        public string NamaPeminjam { get; set; } = string.Empty;

        [Required]
        public string NamaRuangan { get; set; } = string.Empty;

        [Required]
        public string Keperluan { get; set; } = string.Empty;

        [Required]
        public DateTime TanggalPinjaman { get; set; }

        [Required]
        public TimeSpan JamMulai { get; set; }

        [Required]
        public TimeSpan JamSelesai { get; set; }

        // optional: kalau mau bisa dikirim, kalau tidak ya default "menunggu"
        public string? Status { get; set; }
    }
}
