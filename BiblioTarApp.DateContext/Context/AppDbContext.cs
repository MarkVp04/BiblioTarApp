using BiblioTarApp.DataContext.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
