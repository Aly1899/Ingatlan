using Microsoft.EntityFrameworkCore;
using reat.Persistency.Entities;

namespace reat.Persistency
{
    public class AdContext : DbContext
    {
        public DbSet<AdModel> AdModels { get; set; }
        public DbSet<AdPriceModel> AdPriceModels { get; set; }
        public DbSet<FetchDate> FetchDates { get; set; }

        public AdContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdModel>().HasKey(r => r.AdId);
            modelBuilder.Entity<AdPriceModel>().HasKey(a => a.AdPriceId);
        }

    }
}
