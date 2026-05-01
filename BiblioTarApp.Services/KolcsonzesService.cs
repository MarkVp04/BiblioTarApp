using AutoMapper;
using BiblioTarApp.DataContext.Context;
using BiblioTarApp.DataContext.Entites;
using BiblioTarApp.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BiblioTarApp.Services
{
    public interface IKolcsonzesService
    {
        Task<int> Create(KolcsonzesCreateDto kolcsonzesCreateDto);
        Task<List<KolcsonzesGetDto>> List();
        Task<KolcsonzesGetDto> GetById(int id);
        Task<List<KolcsonzesGetDto>> GetByFelhasznaloId(int felhasznaloId);
        Task<string> Hosszabbitas(KolcsonzesHosszabbitasDto kolcsonzesHosszabbitasDto);
        Task<string> UpdateStatus(KolcsonzesStatuszUpdateDto kolcsonzesStatuszUpdateDto);
    }

    public class KolcsonzesService : IKolcsonzesService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public KolcsonzesService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(KolcsonzesCreateDto kolcsonzesCreateDto)
        {
            var foglalas = await _context.Foglalasok
                .Include(f => f.Konyv)
                .Include(f => f.Felhasznalo)
                .Include(f => f.Kolcsonzes)
                .FirstOrDefaultAsync(f => f.Id == kolcsonzesCreateDto.FoglalasId)
                ?? throw new Exception("A foglalás nem található.");

            if (foglalas.Statusz != FoglalasStatusz.Foglalt)
            {
                throw new Exception("Csak aktív foglalásból hozható létre kölcsönzés.");
            }

            if (foglalas.Kolcsonzes != null)
            {
                throw new Exception("Ehhez a foglaláshoz már tartozik kölcsönzés.");
            }

            if (foglalas.Hatarido <= DateTime.Now)
            {
                throw new Exception("A kölcsönzés nem hozható létre, mert a határidő már lejárt.");
            }

            var aktivKolcsonzesVan = await _context.Kolcsonzesek.AnyAsync(k =>
                k.BookId == foglalas.KonyvId &&
                k.Statusz == KolcsonzesStatusz.Aktiv);

            if (aktivKolcsonzesVan)
            {
                throw new Exception("Ehhez a könyvhöz már tartozik aktív kölcsönzés.");
            }

            var kolcsonzes = new Kolcsonzes
            {
                FelhasznaloId = foglalas.FelhasznaloId,
                Email = foglalas.Felhasznalo?.Email,
                BookId = foglalas.KonyvId,
                FoglalasId = foglalas.Id,
                KolcsozesIdeje = DateTime.Now,
                Statusz = KolcsonzesStatusz.Aktiv
            };

            foglalas.Konyv.Statusz = KonyvStatusz.NemElerheto.ToString();

            await _context.Kolcsonzesek.AddAsync(kolcsonzes);
            _context.Konyvek.Update(foglalas.Konyv);

            await _context.SaveChangesAsync();

            return kolcsonzes.Id;
        }

        public async Task<List<KolcsonzesGetDto>> List()
        {
            var kolcsonzesek = await _context.Kolcsonzesek
                .Include(k => k.Konyv)
                .Include(k => k.Foglalas)
                .OrderByDescending(k => k.KolcsozesIdeje)
                .ToListAsync();

            return _mapper.Map<List<KolcsonzesGetDto>>(kolcsonzesek);
        }

        public async Task<KolcsonzesGetDto> GetById(int id)
        {
            var kolcsonzes = await _context.Kolcsonzesek
                .Include(k => k.Konyv)
                .Include(k => k.Foglalas)
                .FirstOrDefaultAsync(k => k.Id == id)
                ?? throw new Exception("A kölcsönzés nem található.");

            return _mapper.Map<KolcsonzesGetDto>(kolcsonzes);
        }

        public async Task<List<KolcsonzesGetDto>> GetByFelhasznaloId(int felhasznaloId)
        {
            var felhasznaloLetezik = await _context.Felhasznalok
                .AnyAsync(f => f.Id == felhasznaloId);

            if (!felhasznaloLetezik)
            {
                throw new Exception("A felhasználó nem található.");
            }

            var kolcsonzesek = await _context.Kolcsonzesek
                .Include(k => k.Konyv)
                .Include(k => k.Foglalas)
                .Where(k => k.FelhasznaloId == felhasznaloId)
                .OrderByDescending(k => k.KolcsozesIdeje)
                .ToListAsync();

            return _mapper.Map<List<KolcsonzesGetDto>>(kolcsonzesek);
        }

        public async Task<string> Hosszabbitas(KolcsonzesHosszabbitasDto kolcsonzesHosszabbitasDto)
        {
            var kolcsonzes = await _context.Kolcsonzesek
                .Include(k => k.Foglalas)
                .FirstOrDefaultAsync(k => k.Id == kolcsonzesHosszabbitasDto.Id)
                ?? throw new Exception("A kölcsönzés nem található.");

            if (kolcsonzes.Statusz != KolcsonzesStatusz.Aktiv)
            {
                throw new Exception("Csak aktív kölcsönzés hosszabbítható.");
            }

            if (kolcsonzes.Foglalas.MeghosszabbitasiLehetosegek < 1)
            {
                throw new Exception("Nincs több hosszabbítási lehetőség.");
            }

            if (kolcsonzesHosszabbitasDto.UjHatarido <= kolcsonzes.Foglalas.Hatarido)
            {
                throw new Exception("Az új határidőnek későbbinek kell lennie a jelenlegi határidőnél.");
            }

            if (kolcsonzesHosszabbitasDto.UjHatarido > kolcsonzes.Foglalas.Hatarido.AddDays(30))
            {
                throw new Exception("Egy hosszabbítás legfeljebb 30 nappal tolhatja ki a határidőt.");
            }

            kolcsonzes.Foglalas.Hatarido = kolcsonzesHosszabbitasDto.UjHatarido;
            kolcsonzes.Foglalas.MeghosszabbitasiLehetosegek--;

            _context.Foglalasok.Update(kolcsonzes.Foglalas);
            await _context.SaveChangesAsync();

            return "A kölcsönzés határideje sikeresen meghosszabbítva.";
        }

        public async Task<string> UpdateStatus(KolcsonzesStatuszUpdateDto kolcsonzesStatuszUpdateDto)
        {
            var kolcsonzes = await _context.Kolcsonzesek
                .Include(k => k.Konyv)
                .Include(k => k.Foglalas)
                .FirstOrDefaultAsync(k => k.Id == kolcsonzesStatuszUpdateDto.Id)
                ?? throw new Exception("A kölcsönzés nem található.");

            kolcsonzes.Statusz = kolcsonzesStatuszUpdateDto.Statusz;

            switch (kolcsonzes.Statusz)
            {
                case KolcsonzesStatusz.Aktiv:
                    kolcsonzes.VisszahozasIdeje = null;
                    kolcsonzes.Konyv.Statusz = KonyvStatusz.NemElerheto.ToString();
                    kolcsonzes.Foglalas.Statusz = FoglalasStatusz.Foglalt;
                    break;

                case KolcsonzesStatusz.Teljesitett:
                    kolcsonzes.VisszahozasIdeje = DateTime.Now;
                    kolcsonzes.Konyv.Statusz = KonyvStatusz.Elerheto.ToString();
                    kolcsonzes.Foglalas.Statusz = FoglalasStatusz.Visszahozott;
                    break;

                case KolcsonzesStatusz.Torolve:
                    kolcsonzes.VisszahozasIdeje = null;
                    kolcsonzes.Konyv.Statusz = KonyvStatusz.Elerheto.ToString();
                    kolcsonzes.Foglalas.Statusz = FoglalasStatusz.Torolve;
                    break;
            }

            _context.Kolcsonzesek.Update(kolcsonzes);
            _context.Konyvek.Update(kolcsonzes.Konyv);
            _context.Foglalasok.Update(kolcsonzes.Foglalas);

            await _context.SaveChangesAsync();

            return $"A kölcsönzés státusza sikeresen módosítva lett erre: {kolcsonzes.Statusz}.";
        }
    }
}