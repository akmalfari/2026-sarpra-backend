namespace Sarpra.Api.Dtos
{
    public class RuanganCreateDto
    {
        public string NamaRuangan { get; set; } = string.Empty;
        public string Keterangan { get; set; } = string.Empty;
        public string Status { get; set; } = "tersedia";
    }
}
