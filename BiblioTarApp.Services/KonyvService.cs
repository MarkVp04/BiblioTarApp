using BiblioTarApp.DataContext.Context;
using BiblioTarApp.DataContext.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioTarApp.Services
{
    public interface IKonyvServices
    {
        List<Konyv> List();
    }
    public class KonyvService : IKonyvServices
    {
        private readonly AppDbContext _context;

        public KonyvService(AppDbContext context)
        {
            _context = context;
        }

        public List<Konyv> List()
        {
            return _context.Konyvek.ToList();
        }
    }
}
