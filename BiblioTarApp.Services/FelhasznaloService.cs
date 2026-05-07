using AutoMapper;
using BiblioTarApp.DataContext.Context;
using BiblioTarApp.DataContext.Entites;
using BiblioTarApp.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BiblioTarApp.Services
{
    public interface IFelhasznaloService
    {
        Task<int> Create(FelhasznaloCreateDto felhasznaloCreateDto);
        Task<List<FelhasznaloGetDto>> List();
        Task<FelhasznaloGetDto> GetById(int id);
        Task<string> Update(FelhasznaloUpdateDto felhasznaloUpdateDto);
        Task<string> SoftDelete(int id);
        Task<string> UpdateSzerepkor(FelhasznaloSzerepkorUpdateDto felhasznaloSzerepkorUpdateDto);
        Task<LoginResponseDto> Login(FelhasznaloLoginDto felhasznaloLoginDto);
        Task<Felhasznalo> Authenticate(FelhasznaloLoginDto felhasznaloLoginDto);
        string GenerateToken(Felhasznalo felhasznalo);
    }

    public class FelhasznaloService : IFelhasznaloService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public FelhasznaloService(AppDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<int> Create(FelhasznaloCreateDto felhasznaloCreateDto)
        {
            if (string.IsNullOrWhiteSpace(felhasznaloCreateDto.Nev))
                throw new Exception("A név megadása kötelező.");

            if (string.IsNullOrWhiteSpace(felhasznaloCreateDto.Email))
                throw new Exception("Az email megadása kötelező.");

            if (string.IsNullOrWhiteSpace(felhasznaloCreateDto.Jelszo))
                throw new Exception("A jelszó megadása kötelező.");

            var emailLetezik = await _context.Felhasznalok
                .AnyAsync(f => f.Email == felhasznaloCreateDto.Email);

            if (emailLetezik)
                throw new Exception("Ezzel az email címmel már létezik felhasználó.");

            var felhasznalo = _mapper.Map<Felhasznalo>(felhasznaloCreateDto);

            felhasznalo.Jelszo = BCrypt.Net.BCrypt.HashPassword(felhasznaloCreateDto.Jelszo);

            if (felhasznaloCreateDto.Szerepkor != 0)
            {
                felhasznalo.Szerepkor = felhasznaloCreateDto.Szerepkor;
            }
            else
            {
                felhasznalo.Szerepkor = Felhasznalo.Beosztas.Regisztralt;
            }

            felhasznalo.Aktiv = true;

            await _context.Felhasznalok.AddAsync(felhasznalo);
            await _context.SaveChangesAsync();

            return felhasznalo.Id;
        }

        public async Task<LoginResponseDto> Login(FelhasznaloLoginDto felhasznaloLoginDto)
        {
            var felhasznalo = await Authenticate(felhasznaloLoginDto);

            return new LoginResponseDto
            {
                Token = GenerateToken(felhasznalo),
                FelhasznaloId = felhasznalo.Id,
                Nev = felhasznalo.Nev,
                Email = felhasznalo.Email,
                Szerepkor = felhasznalo.Szerepkor.ToString() // Itt szövegként megy vissza: "Konyvtaros"
            };
        }

        public async Task<Felhasznalo> Authenticate(FelhasznaloLoginDto felhasznaloLoginDto)
        {
            var felhasznalo = await _context.Felhasznalok
                .Include(f => f.Lakcimek)
                .FirstOrDefaultAsync(f => f.Email == felhasznaloLoginDto.Email)
                ?? throw new Exception("A felhasználó nem található.");

            if (!felhasznalo.Aktiv)
                throw new Exception("A felhasználó inaktív.");

            bool helyesJelszo = BCrypt.Net.BCrypt.Verify(felhasznaloLoginDto.Jelszo, felhasznalo.Jelszo);

            if (!helyesJelszo)
                throw new Exception("Hibás jelszó.");

            return felhasznalo;
        }

        public string GenerateToken(Felhasznalo felhasznalo)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, felhasznalo.Id.ToString()),
                new Claim(ClaimTypes.Name, felhasznalo.Nev),
                new Claim(ClaimTypes.Email, felhasznalo.Email),
                // Fontos: A Role claim-be a szöveges megnevezés kerül (pl. "Konyvtaros")
                new Claim(ClaimTypes.Role, felhasznalo.Szerepkor.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expires = DateTime.Now.AddDays(
                Convert.ToDouble(_configuration["JwtSettings:ExpiresInDays"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<List<FelhasznaloGetDto>> List()
        {
            var felhasznalok = await _context.Felhasznalok
                .Include(f => f.Lakcimek)
                .Where(f => f.Aktiv)
                .OrderBy(f => f.Nev)
                .ToListAsync();

            return _mapper.Map<List<FelhasznaloGetDto>>(felhasznalok);
        }

        public async Task<FelhasznaloGetDto> GetById(int id)
        {
            var felhasznalo = await _context.Felhasznalok
                .Include(f => f.Lakcimek)
                .FirstOrDefaultAsync(f => f.Id == id)
                ?? throw new Exception("A felhasználó nem található.");

            return _mapper.Map<FelhasznaloGetDto>(felhasznalo);
        }

        public async Task<string> Update(FelhasznaloUpdateDto felhasznaloUpdateDto)
        {
            var felhasznalo = await _context.Felhasznalok
                .FirstOrDefaultAsync(f => f.Id == felhasznaloUpdateDto.Id)
                ?? throw new Exception("A felhasználó nem található.");

            if (!felhasznalo.Aktiv)
                throw new Exception("Inaktív felhasználó nem módosítható.");

            if (!string.IsNullOrWhiteSpace(felhasznaloUpdateDto.Email) &&
                felhasznaloUpdateDto.Email != felhasznalo.Email)
            {
                var emailFoglalt = await _context.Felhasznalok
                    .AnyAsync(f => f.Email == felhasznaloUpdateDto.Email && f.Id != felhasznaloUpdateDto.Id);

                if (emailFoglalt)
                    throw new Exception("Ez az email cím már foglalt.");

                felhasznalo.Email = felhasznaloUpdateDto.Email;
            }

            if (!string.IsNullOrWhiteSpace(felhasznaloUpdateDto.Nev))
                felhasznalo.Nev = felhasznaloUpdateDto.Nev;

            if (!string.IsNullOrWhiteSpace(felhasznaloUpdateDto.Telefonszam))
                felhasznalo.Telefonszam = felhasznaloUpdateDto.Telefonszam;

            _context.Felhasznalok.Update(felhasznalo);
            await _context.SaveChangesAsync();

            return $"A felhasználó adatai sikeresen módosítva lettek. Azonosító: {felhasznalo.Id}";
        }

        public async Task<string> SoftDelete(int id)
        {
            var felhasznalo = await _context.Felhasznalok
                .FirstOrDefaultAsync(f => f.Id == id)
                ?? throw new Exception("A felhasználó nem található.");

            if (!felhasznalo.Aktiv)
                throw new Exception("A felhasználó már inaktív.");

            felhasznalo.Aktiv = false;

            _context.Felhasznalok.Update(felhasznalo);
            await _context.SaveChangesAsync();

            return $"A felhasználó inaktiválva lett. Azonosító: {felhasznalo.Id}";
        }

        public async Task<string> UpdateSzerepkor(FelhasznaloSzerepkorUpdateDto felhasznaloSzerepkorUpdateDto)
        {
            var felhasznalo = await _context.Felhasznalok
                .FirstOrDefaultAsync(f => f.Id == felhasznaloSzerepkorUpdateDto.Id)
                ?? throw new Exception("A felhasználó nem található.");

            felhasznalo.Szerepkor = felhasznaloSzerepkorUpdateDto.Szerepkor;

            _context.Felhasznalok.Update(felhasznalo);
            await _context.SaveChangesAsync();

            return $"A felhasználó szerepköre sikeresen módosítva lett erre: {felhasznalo.Szerepkor}.";
        }
    }
}