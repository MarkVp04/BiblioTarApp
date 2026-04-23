using BiblioTarApp.DataContext.Entites;
using Microsoft.EntityFrameworkCore;

namespace BiblioTarApp.DataContext.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Buntetes> Buntetesek { get; set; }
        public DbSet<Felhasznalo> Felhasznalok { get; set; }
        public DbSet<Foglalas> Foglalasok { get; set; }
        public DbSet<Kolcsonzes> Kolcsonzesek { get; set; }
        public DbSet<Konyv> Konyvek { get; set; }
        public DbSet<Lakcim> Lakcimek { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Felhasznalo>()
                .HasMany(f => f.Lakcimek)
                .WithOne(l => l.Felhasznalo)
                .HasForeignKey(l => l.FelhasznaloId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Foglalas>()
                .HasOne(f => f.Kolcsonzes)
                .WithOne(k => k.Foglalas)
                .HasForeignKey<Kolcsonzes>(k => k.FoglalasId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Buntetes>()
                .HasOne(b => b.Felhasznalo)
                .WithMany(f => f.Buntetesek)
                .HasForeignKey(b => b.FelhasznaloId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Foglalas>()
                .HasOne(f => f.Szamla)
                .WithOne(b => b.Foglalas)
                .HasForeignKey<Buntetes>(b => b.FoglalasId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}