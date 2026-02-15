namespace Sarpra.Api.Dtos
{
    public class RuanganDto
    {
        public int Id { get; set; }
        public string NamaRuangan { get; set; } = string.Empty;
        public string? KeteranganRuangan { get; set; }
        public bool IsTersedia { get; set; }
    }
}
