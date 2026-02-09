using System.ComponentModel.DataAnnotations;

namespace Sarpra.Api.Dtos
{
    public class PeminjamanUpdateStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;

        public string? DiubahOleh { get; set; }
        public string? Keterangan { get; set; }
    }
}
