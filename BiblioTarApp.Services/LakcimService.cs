using BiblioTarApp.DataContext.Context;
using BiblioTarApp.DataContext.Entites;
using BiblioTarApp.DTOs;

namespace BiblioTarApp.Services
{
    public interface ILakcimService
    {
        Task<int> CreateAsyncLakcim(LakcimCreateDto lakcimCreateDto);
    }

    public class LakcimService : ILakcimService
    {
        private readonly AppDbContext _context;

        public LakcimService(AppDbContext context)
        {
            _context = context;
        }

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
    }
}