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
            CreateMap<Konyv, KonyvGetDto>();
            CreateMap<KonyvCreateDto, Konyv>();
            CreateMap<KonyvUpdateDto, Konyv>();

            // LAKCÍM
            CreateMap<Lakcim, LakcimGetDto>();
            CreateMap<LakcimCreateDto, Lakcim>();
        }
    }
}