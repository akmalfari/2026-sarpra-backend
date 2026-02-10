using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sarpra.Api.Models
{
    [Table("riwayat_status_peminjaman")]
    public class RiwayatStatusPeminjaman
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("peminjaman_id")]
        public int PeminjamanId { get; set; }

        [Required]
        [Column("status_sebelumnya")]
        public string StatusSebelumnya { get; set; } = string.Empty;

        [Required]
        [Column("status_sekarang")]
        public string StatusSekarang { get; set; } = string.Empty;

        [Column("diubah_oleh")]
        public string? DiubahOleh { get; set; }

        [Column("keterangan")]
        public string? Keterangan { get; set; }

        [Required]
        [Column("waktu_perubahan")]
        public DateTime WaktuPerubahan { get; set; }
    }
}
