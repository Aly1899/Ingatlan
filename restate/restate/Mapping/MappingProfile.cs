using Application.Model;
using AutoMapper;
using restate.API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restate.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<AdPrice, AdPriceDTO>();
            //CreateMap<List<AdPrice>, List<AdPriceDTO>>();
        }
    }
}
