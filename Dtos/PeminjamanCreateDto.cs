namespace Sarpra.Api.Dtos
{
    public class PeminjamanCreateDto
    {
        public string NamaPeminjam { get; set; } = string.Empty;
        public string NamaRuangan { get; set; } = string.Empty;
        public string Keperluan { get; set; } = string.Empty;
        public DateTime TanggalPeminjaman { get; set; }
        public TimeSpan JamMulai { get; set; }
        public TimeSpan JamSelesai { get; set; }
    }
}
