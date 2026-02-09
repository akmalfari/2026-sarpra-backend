using System.ComponentModel.DataAnnotations;

namespace Sarpra.Api.Dtos
{
    public class PeminjamanUpdateDto
    {
        [Required]
        public string NamaPeminjam { get; set; } = string.Empty;

        [Required]
        public string NamaRuangan { get; set; } = string.Empty;

        [Required]
        public string Keperluan { get; set; } = string.Empty;

        [Required]
        public DateTime TanggalPeminjaman { get; set; }

        [Required]
        public TimeSpan JamMulai { get; set; }

        [Required]
        public TimeSpan JamSelesai { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
