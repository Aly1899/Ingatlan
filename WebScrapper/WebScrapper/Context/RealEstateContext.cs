using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebScrapper.Models;

namespace WebScrapper.Context
{
    class RealEstateContext: DbContext
    {
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<FetchDate> FetchDates { get; set; }
        public DbSet<AdPrice> AdPrices { get; set; }

        public RealEstateContext(DbContextOptions<RealEstateContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RealEstate>().HasKey(r=>r.RealEstateId);
            modelBuilder.Entity<FetchDate>().HasKey(f=>f.Id);
            modelBuilder.Entity<AdPrice>().HasKey(a=>a.AdPriceId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=ingatlan;User Id=postgres;Password=admin;");
        }
    }
}
