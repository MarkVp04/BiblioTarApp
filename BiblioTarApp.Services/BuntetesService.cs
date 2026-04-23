using AutoMapper;
using BiblioTarApp.DataContext.Context;
using BiblioTarApp.DataContext.Entites;
using BiblioTarApp.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BiblioTarApp.Services
{
    public interface IBuntetesService
    {
        Task<int> Create(BuntetesCreateDto buntetesCreateDto);
        Task<List<BuntetesGetDto>> List();
        Task<BuntetesGetDto> GetById(int id);
        Task<List<BuntetesGetDto>> GetByFelhasznaloId(int felhasznaloId);
        Task<string> UpdateFizetesiStatusz(BuntetesFizetesiStatuszUpdateDto buntetesFizetesiStatuszUpdateDto);
    }

    public class BuntetesService : IBuntetesService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BuntetesService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(BuntetesCreateDto buntetesCreateDto)
        {
            if (buntetesCreateDto.Ar <= 0)
            {
                throw new Exception("A büntetés összege csak pozitív szám lehet.");
            }

            var felhasznalo = await _context.Felhasznalok
                .FirstOrDefaultAsync(f => f.Id == buntetesCreateDto.FelhasznaloId)
                ?? throw new Exception("A felhasználó nem található.");

            var foglalas = await _context.Foglalasok
                .Include(f => f.Konyv)
                .Include(f => f.Kolcsonzes)
                .Include(f => f.Szamla)
                .FirstOrDefaultAsync(f => f.Id == buntetesCreateDto.FoglalasId)
                ?? throw new Exception("A foglalás nem található.");

            if (foglalas.FelhasznaloId != buntetesCreateDto.FelhasznaloId)
            {
                throw new Exception("A megadott foglalás nem ehhez a felhasználóhoz tartozik.");
            }

            if (foglalas.Statusz != FoglalasStatusz.Visszahozott)
            {
                throw new Exception("Büntetés csak visszahozott könyvhöz rögzíthető.");
            }

            if (foglalas.Hatarido >= DateTime.Now)
            {
                throw new Exception("A könyv nem késve lett visszahozva, ezért nem adható hozzá büntetés.");
            }

            if (foglalas.Szamla != null)
            {
                throw new Exception("Ehhez a foglaláshoz már tartozik büntetés.");
            }

            var buntetes = new Buntetes
            {
                FelhasznaloId = felhasznalo.Id,
                FoglalasId = foglalas.Id,
                Ar = buntetesCreateDto.Ar,
                FizetesiStatusz = false,
                BuntetesIdeje = DateTime.Now
            };

            await _context.Buntetesek.AddAsync(buntetes);
            await _context.SaveChangesAsync();

            return buntetes.Id;
        }

        public async Task<List<BuntetesGetDto>> List()
        {
            var buntetesek = await _context.Buntetesek
                .Include(b => b.Foglalas)
                    .ThenInclude(f => f.Konyv)
                .OrderByDescending(b => b.BuntetesIdeje)
                .ToListAsync();

            return _mapper.Map<List<BuntetesGetDto>>(buntetesek);
        }

        public async Task<BuntetesGetDto> GetById(int id)
        {
            var buntetes = await _context.Buntetesek
                .Include(b => b.Foglalas)
                    .ThenInclude(f => f.Konyv)
                .FirstOrDefaultAsync(b => b.Id == id)
                ?? throw new Exception("A büntetés nem található.");

            return _mapper.Map<BuntetesGetDto>(buntetes);
        }

        public async Task<List<BuntetesGetDto>> GetByFelhasznaloId(int felhasznaloId)
        {
            var felhasznaloLetezik = await _context.Felhasznalok
                .AnyAsync(f => f.Id == felhasznaloId);

            if (!felhasznaloLetezik)
            {
                throw new Exception("A felhasználó nem található.");
            }

            var buntetesek = await _context.Buntetesek
                .Include(b => b.Foglalas)
                    .ThenInclude(f => f.Konyv)
                .Where(b => b.FelhasznaloId == felhasznaloId)
                .OrderByDescending(b => b.BuntetesIdeje)
                .ToListAsync();

            return _mapper.Map<List<BuntetesGetDto>>(buntetesek);
        }

        public async Task<string> UpdateFizetesiStatusz(BuntetesFizetesiStatuszUpdateDto buntetesFizetesiStatuszUpdateDto)
        {
            var buntetes = await _context.Buntetesek
                .FirstOrDefaultAsync(b => b.Id == buntetesFizetesiStatuszUpdateDto.Id)
                ?? throw new Exception("A büntetés nem található.");

            buntetes.FizetesiStatusz = buntetesFizetesiStatuszUpdateDto.FizetesiStatusz;

            _context.Buntetesek.Update(buntetes);
            await _context.SaveChangesAsync();

            return $"A büntetés fizetési státusza sikeresen módosítva lett erre: {buntetes.FizetesiStatusz}.";
        }
    }
}