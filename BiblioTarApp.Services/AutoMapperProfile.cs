using AutoMapper;
using BiblioTarApp.DataContext.Entites;
using BiblioTarApp.DTOs;

namespace BiblioTarApp.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // KÖNYV
            CreateMap<Konyv, KonyvGetDto>().ReverseMap();
            CreateMap<KonyvCreateDto, Konyv>().ReverseMap();
            CreateMap<KonyvUpdateDto, Konyv>().ReverseMap();

            // LAKCÍM
            CreateMap<Lakcim, LakcimGetDto>().ReverseMap();
            CreateMap<LakcimCreateDto, Lakcim>().ReverseMap();

            // FELHASZNÁLÓ
            CreateMap<Felhasznalo, FelhasznaloGetDto>().ReverseMap();
            CreateMap<FelhasznaloCreateDto, Felhasznalo>().ReverseMap();

            // FOGLALÁS
            CreateMap<Foglalas, FoglalasGetDto>()
                .ForMember(dest => dest.KonyvCim, opt => opt.MapFrom(src => src.Konyv.Cim));
            CreateMap<FoglalasCreateDto, Foglalas>();

            // KÖLCSÖNZÉS
            CreateMap<Kolcsonzes, KolcsonzesGetDto>()
                .ForMember(dest => dest.KonyvId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.KonyvCim, opt => opt.MapFrom(src => src.Konyv.Cim))
                .ForMember(dest => dest.Hatarido, opt => opt.MapFrom(src => src.Foglalas.Hatarido))
                .ForMember(dest => dest.MeghosszabbitasiLehetosegek, opt => opt.MapFrom(src => src.Foglalas.MeghosszabbitasiLehetosegek));

            // BÜNTETÉS
            CreateMap<Buntetes, BuntetesGetDto>()
                .ForMember(dest => dest.KonyvCim, opt => opt.MapFrom(src => src.Foglalas.Konyv.Cim));
        }
    }
}