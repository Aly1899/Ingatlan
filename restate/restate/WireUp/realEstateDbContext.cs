//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using restate.Application.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace restate.WireUp
//{
//    public class RealEstateDbContext : DbContext
//    {

//        public RealEstateDbContext(DbContextOptions options) : base(options)
//        {
//        }

//        public DbSet<RealEstate> RealEstates { get; set; }
//        public DbSet<AdChange> AdChanges { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.HasDefaultSchema("ingatlan");
//            modelBuilder.Entity<RealEstate>().HasNoKey();
//            modelBuilder.Entity<AdChange>().HasKey(a => new { a.Id });
//        }

//    }
//}
