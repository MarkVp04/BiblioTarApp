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
            CreateMap<KonyvUpdateDto, Konyv>().ReverseMap(); ;

            // LAKCÍM
            CreateMap<Lakcim, LakcimGetDto>().ReverseMap();
            CreateMap<LakcimCreateDto, Lakcim>().ReverseMap();
        }
    }
}