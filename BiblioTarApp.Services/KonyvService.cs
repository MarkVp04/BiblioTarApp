using AutoMapper;
using BiblioTarApp.DataContext.Context;
using BiblioTarApp.DataContext.Entites;
using BiblioTarApp.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BiblioTarApp.Services
{
    public interface IKonyvServices
    {
        Task<int> Create(KonyvCreateDto konyvCreateDto);
        Task<bool> Delete(int id);
        Task<List<KonyvGetDto>> List();
        Task<KonyvGetDto> GetById(int id);
        Task<string> Update(KonyvUpdateDto konyvUpdateDto);
    }

    public class KonyvService : IKonyvServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public KonyvService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Delete(int id)
        {
            var konyv = await _context.Konyvek.FindAsync(id) ?? throw new Exception("A könyv nem található");
            _context.Konyvek.Remove(konyv);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<KonyvGetDto>> List()
        {
            var konyvek = await _context.Konyvek.ToListAsync();
            return _mapper.Map<List<KonyvGetDto>>(konyvek);
        }

        public async Task<KonyvGetDto> GetById(int id)
        {
            var konyv = await _context.Konyvek.FirstOrDefaultAsync(x => x.Id == id)
                        ?? throw new Exception("A könyv nem található");

            return _mapper.Map<KonyvGetDto>(konyv);
        }

        public async Task<int> Create(KonyvCreateDto konyvCreateDto)
        {
            var konyv = _mapper.Map<Konyv>(konyvCreateDto);

            if (string.IsNullOrWhiteSpace(konyv.Statusz))
            {
                konyv.Statusz = KonyvStatusz.Elerheto.ToString();
            }

            if (string.IsNullOrWhiteSpace(konyv.Allapot))
            {
                konyv.Allapot = KonyvAllapot.Hibatlan.ToString();
            }

            await _context.Konyvek.AddAsync(konyv);
            await _context.SaveChangesAsync();
            return konyv.Id;
        }

        public async Task<string> Update(KonyvUpdateDto konyvUpdateDto)
        {
            var konyv = await _context.Konyvek.FindAsync(konyvUpdateDto.Id)
                        ?? throw new Exception("A könyv nem található");

            konyv.Cim = konyvUpdateDto.Cim;
            konyv.Szerzo = konyvUpdateDto.Szerzo;
            konyv.Isbn = konyvUpdateDto.Isbn;
            konyv.Kategoria = konyvUpdateDto.Kategoria;
            konyv.Kiadasev = konyvUpdateDto.Kiadasev;
            konyv.Allapot = konyvUpdateDto.Allapot;
            konyv.Statusz = konyvUpdateDto.Statusz;
            konyv.PublikalasIdeje = konyvUpdateDto.PublikalasIdeje;
            konyv.Ertelekes = konyvUpdateDto.Ertelekes;

            konyv.Kolcsonozheto = konyvUpdateDto.Kolcsonozheto;

            _context.Konyvek.Update(konyv);
            await _context.SaveChangesAsync();

            return $"A könyv azonosítója {konyv.Id}, sikeresen módosítva.";
        }
    }
}