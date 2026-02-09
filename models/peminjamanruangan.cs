using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sarpra.Api.Models
{
    [Table("peminjaman_ruangan")]
    public class PeminjamanRuangan
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nama_peminjam")]
        public string NamaPeminjam { get; set; } = string.Empty;

        [Required]
        [Column("nama_ruangan")]
        public string NamaRuangan { get; set; } = string.Empty;

        [Required]
        [Column("keperluan")]
        public string Keperluan { get; set; } = string.Empty;

        [Required]
        [Column("tanggal_peminjaman")]
        public DateTime TanggalPeminjaman { get; set; }

        [Required]
        [Column("jam_mulai")]
        public TimeSpan JamMulai { get; set; }

        [Required]
        [Column("jam_selesai")]
        public TimeSpan JamSelesai { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; } = "menunggu";
    }
}
