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
            // Ez a kód kikerüli a bonyolult típuskonverziós hibákat a teszt idejére
            return _context.Konyvek
                .Select(k => new Konyv
                {
                    Id = k.Id,
                    Cim = k.Cim,
                    Szerzo = k.Szerzo,
                    Kiadasev = k.Kiadasev,
                    Isbn = k.Isbn
                })
                .ToList();
        }
    }
}
