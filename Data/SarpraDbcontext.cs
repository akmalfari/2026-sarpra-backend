using Microsoft.EntityFrameworkCore;
using Sarpra.Api.Models;

namespace Sarpra.Api.Data
{
    public class SarpraDbContext : DbContext
    {
        public SarpraDbContext(DbContextOptions<SarpraDbContext> options) : base(options) { }

        public DbSet<PeminjamanRuangan> PeminjamanRuangan { get; set; }
        public DbSet<Ruangan> Ruangan { get; set; }
    }
}
