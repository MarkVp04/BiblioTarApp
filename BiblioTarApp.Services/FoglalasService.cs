using AutoMapper;
using BiblioTarApp.DataContext.Context;
using BiblioTarApp.DataContext.Entites;
using BiblioTarApp.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BiblioTarApp.Services
{
    public interface IFoglalasService
    {
        Task<int> Create(FoglalasCreateDto foglalasCreateDto);
        Task<List<FoglalasGetDto>> List();
        Task<FoglalasGetDto> GetById(int id);
        Task<List<FoglalasGetDto>> GetByFelhasznaloId(int felhasznaloId);
        Task<string> UpdateStatus(FoglalasStatuszUpdateDto foglalasStatuszUpdateDto);
        Task<string> Cancel(int id);
    }

    public class FoglalasService : IFoglalasService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FoglalasService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(FoglalasCreateDto foglalasCreateDto)
        {
            var felhasznalo = await _context.Felhasznalok
                .FirstOrDefaultAsync(f => f.Id == foglalasCreateDto.FelhasznaloId)
                ?? throw new Exception("A felhasználó nem található.");

            var konyv = await _context.Konyvek
                .FirstOrDefaultAsync(k => k.Id == foglalasCreateDto.KonyvId)
                ?? throw new Exception("A könyv nem található.");

            if (foglalasCreateDto.Hatarido <= DateTime.Now)
            {
                throw new Exception("A határidőnek későbbi időpontnak kell lennie, mint a foglalás ideje.");
            }

            var aktivFoglalasVan = await _context.Foglalasok.AnyAsync(f =>
                f.KonyvId == foglalasCreateDto.KonyvId &&
                f.Statusz == FoglalasStatusz.Foglalt);

            if (aktivFoglalasVan)
            {
                throw new Exception("Ehhez a könyvhöz már tartozik aktív foglalás.");
            }

            var aktualisStatusz = konyv.Statusz?.Trim();

            if (!string.IsNullOrWhiteSpace(aktualisStatusz) &&
                aktualisStatusz != KonyvStatusz.Elerheto.ToString())
            {
                throw new Exception("A könyv jelenleg nem foglalható.");
            }

            var foglalas = new Foglalas
            {
                FelhasznaloId = felhasznalo.Id,
                KonyvId = konyv.Id,
                FoglalasIdeje = DateTime.Now,
                Hatarido = foglalasCreateDto.Hatarido,
                MeghosszabbitasiLehetosegek = 2,
                Statusz = FoglalasStatusz.Foglalt
            };

            konyv.Statusz = KonyvStatusz.Foglalt.ToString();

            await _context.Foglalasok.AddAsync(foglalas);
            _context.Konyvek.Update(konyv);

            await _context.SaveChangesAsync();

            return foglalas.Id;
        }

        public async Task<List<FoglalasGetDto>> List()
        {
            var foglalasok = await _context.Foglalasok
                .Include(f => f.Konyv)
                .OrderByDescending(f => f.FoglalasIdeje)
                .ToListAsync();

            return _mapper.Map<List<FoglalasGetDto>>(foglalasok);
        }

        public async Task<FoglalasGetDto> GetById(int id)
        {
            var foglalas = await _context.Foglalasok
                .Include(f => f.Konyv)
                .FirstOrDefaultAsync(f => f.Id == id)
                ?? throw new Exception("A foglalás nem található.");

            return _mapper.Map<FoglalasGetDto>(foglalas);
        }

        public async Task<List<FoglalasGetDto>> GetByFelhasznaloId(int felhasznaloId)
        {
            var felhasznaloLetezik = await _context.Felhasznalok
                .AnyAsync(f => f.Id == felhasznaloId);

            if (!felhasznaloLetezik)
            {
                throw new Exception("A felhasználó nem található.");
            }

            var foglalasok = await _context.Foglalasok
                .Include(f => f.Konyv)
                .Where(f => f.FelhasznaloId == felhasznaloId)
                .OrderByDescending(f => f.FoglalasIdeje)
                .ToListAsync();

            return _mapper.Map<List<FoglalasGetDto>>(foglalasok);
        }

        public async Task<string> UpdateStatus(FoglalasStatuszUpdateDto foglalasStatuszUpdateDto)
        {
            var foglalas = await _context.Foglalasok
                .Include(f => f.Konyv)
                .Include(f => f.Kolcsonzes)
                .FirstOrDefaultAsync(f => f.Id == foglalasStatuszUpdateDto.Id)
                ?? throw new Exception("A foglalás nem található.");

            if (foglalas.Kolcsonzes != null &&
                foglalasStatuszUpdateDto.Statusz == FoglalasStatusz.Torolve)
            {
                throw new Exception("A foglaláshoz már tartozik kölcsönzés, ezért nem törölhető.");
            }

            foglalas.Statusz = foglalasStatuszUpdateDto.Statusz;

            if (foglalas.Statusz == FoglalasStatusz.Foglalt)
            {
                foglalas.Konyv.Statusz = KonyvStatusz.Foglalt.ToString();
            }
            else
            {
                foglalas.Konyv.Statusz = KonyvStatusz.Elerheto.ToString();
            }

            _context.Foglalasok.Update(foglalas);
            _context.Konyvek.Update(foglalas.Konyv);

            await _context.SaveChangesAsync();

            return $"A foglalás státusza sikeresen módosítva lett erre: {foglalas.Statusz}.";
        }

        public async Task<string> Cancel(int id)
        {
            var foglalas = await _context.Foglalasok
                .Include(f => f.Konyv)
                .Include(f => f.Kolcsonzes)
                .FirstOrDefaultAsync(f => f.Id == id)
                ?? throw new Exception("A foglalás nem található.");

            if (foglalas.Kolcsonzes != null)
            {
                throw new Exception("A foglaláshoz már tartozik kölcsönzés, ezért nem törölhető.");
            }

            foglalas.Statusz = FoglalasStatusz.Torolve;
            foglalas.Konyv.Statusz = KonyvStatusz.Elerheto.ToString();

            _context.Foglalasok.Update(foglalas);
            _context.Konyvek.Update(foglalas.Konyv);

            await _context.SaveChangesAsync();

            return "A foglalás sikeresen törölve lett.";
        }
    }
}