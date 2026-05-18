using BiblioTarApp.DataContext.Context;
using BiblioTarApp.DataContext.Entites;
using BiblioTarApp.DTOs;
using Microsoft.EntityFrameworkCore; // Ez kell a FirstOrDefaultAsync-hoz!

namespace BiblioTarApp.Services
{
    public interface ILakcimService
    {
        Task<int> CreateAsyncLakcim(LakcimCreateDto lakcimCreateDto);
        
        // --- ÚJ INTERFACE METÓDUS ---
        Task<int> CreateOrUpdateSingleAsync(LakcimCreateDto lakcimCreateDto);
    }

    public class LakcimService : ILakcimService
    {
        private readonly AppDbContext _context;

        public LakcimService(AppDbContext context)
        {
            _context = context;
        }

        // Megtartjuk a régit is, ha máshol kellene
        public async Task<int> CreateAsyncLakcim(LakcimCreateDto lakcimCreateDto)
        {
            var lakcim = new Lakcim
            {
                Iranyitoszam = lakcimCreateDto.Iranyitoszam,
                Varos = lakcimCreateDto.Varos,
                Utca = lakcimCreateDto.Utca,
                hazszam = lakcimCreateDto.Hazszam,
                FelhasznaloId = lakcimCreateDto.FelhasznaloId
            };

            await _context.Lakcimek.AddAsync(lakcim);
            await _context.SaveChangesAsync();

            return lakcim.Id;
        }

        // --- AZ ÚJ UPSERT LOGIKA ---
        public async Task<int> CreateOrUpdateSingleAsync(LakcimCreateDto lakcimCreateDto)
        {
            // Megnézzük, van-e már lakcíme a felhasználónak
            var existing = await _context.Lakcimek
                .FirstOrDefaultAsync(l => l.FelhasznaloId == lakcimCreateDto.FelhasznaloId);

            if (existing == null)
            {
                // INSERT: Ha nincs, létrehozzuk
                var lakcim = new Lakcim
                {
                    Iranyitoszam = lakcimCreateDto.Iranyitoszam,
                    Varos = lakcimCreateDto.Varos,
                    Utca = lakcimCreateDto.Utca,
                    hazszam = lakcimCreateDto.Hazszam,
                    FelhasznaloId = lakcimCreateDto.FelhasznaloId
                };

                await _context.Lakcimek.AddAsync(lakcim);
                await _context.SaveChangesAsync();
                return lakcim.Id;
            }

            // UPDATE: Ha van, frissítjük az adatait
            existing.Iranyitoszam = lakcimCreateDto.Iranyitoszam;
            existing.Varos = lakcimCreateDto.Varos;
            existing.Utca = lakcimCreateDto.Utca;
            existing.hazszam = lakcimCreateDto.Hazszam;

            _context.Lakcimek.Update(existing);
            await _context.SaveChangesAsync();

            return existing.Id;
        }
    }
}