using BiblioTarApp.DataContext.Context;
using BiblioTarApp.DataContext.Entites;

namespace BiblioTarApp.Services;

public interface IFelhasznaloService
{
    Felhasznalo? Bejelentkezes(string email, string jelszo);
}

public class FelhasznaloService : IFelhasznaloService
{
    private readonly AppDbContext _context;

    public FelhasznaloService(AppDbContext context)
    {
        _context = context;
    }

    public Felhasznalo? Bejelentkezes(string email, string jelszo)
    {
        var user = _context.Felhasznalok
            .FirstOrDefault(f => f.Email == email && f.Jelszo == jelszo);
        return user;
    }
}
