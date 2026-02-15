using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sarpra.Api.Models
{
    [Table("ruangan")]
    public class Ruangan
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nama_ruangan")]
        public string NamaRuangan { get; set; } = string.Empty;

        [Required]
        [Column("keterangan")]
        public string Keterangan { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        public string Status { get; set; } = "tersedia";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
