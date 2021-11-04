using System;
using Application.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace restate.Context
{
    public partial class RealEstateContext : DbContext
    {
        public RealEstateContext()
        {
        }

        public RealEstateContext(DbContextOptions<RealEstateContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdPrice> AdPrices { get; set; }
        public virtual DbSet<RealEstate> RealEstates { get; set; }
        public virtual DbSet<FetchDate> FetchDates { get; set; }
        public virtual DbSet<WithPrice> WithPrices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Initial Catalog=realestate;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AdPrice>(entity =>
            {
                entity.Property(e => e.AdPriceId).ValueGeneratedNever();

                entity.Property(e => e.OldPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.NewPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<RealEstate>(entity =>
            {
                entity.HasKey(e => e.AdId);

                entity.Property(e => e.Area).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PlotSize).HasColumnType("int");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PricePerSqm).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<FetchDate>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<WithPrice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("with_price");

                entity.Property(e => e.AdId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
