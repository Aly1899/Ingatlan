using AutoMapper;
using reat.DTO;
using reat.Persistency.Entities;

namespace reat.Mappers
{
    public class AdMapperProfile : Profile
    {
        public AdMapperProfile()
        {
            CreateMap<AdModel, GetAdsQueryResult>();
        }
    }
}
