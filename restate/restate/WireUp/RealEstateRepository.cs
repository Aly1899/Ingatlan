using Application.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using restate.API.DTO;
using restate.Application.Contracts;
using restate.Context;
using restate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restate.WireUp
{
    public class RealEstateRepository : IRealEstateRepository
    {
        private readonly RealEstateContext _repoRealEstate;
        private readonly IMapper _mapper;

        public RealEstateRepository(RealEstateContext repoRealEsteta, IMapper mapper)
        {
            _repoRealEstate = repoRealEsteta;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<RealEstate>> GetRealEstates()
        {
            var result =await _repoRealEstate.RealEstates
                                    //.Where(r => r.Address.Contains("Nagykáta"))
                                    .ToListAsync();

            return result;
        }

        public async Task<RealEstate> GetRealEstateById(Guid realEstateId)
        {
            var result = await _repoRealEstate.RealEstates
                                    .FirstOrDefaultAsync(r => r.RealEstateId == realEstateId);
            var adPrice = await _repoRealEstate.AdPrices.Where(a => a.RealEstateId == realEstateId).ToListAsync();
            var adPriceDTO = _mapper.Map<AdPriceDTO>(adPrice.First());
            return result;
        }

        public async Task<RealEstateDTO> GetRealEstateFull(Guid realEstateId)
        {

            var result = new RealEstateDTO(); 
            var re = await _repoRealEstate.RealEstates
                                    .FirstOrDefaultAsync(r => r.RealEstateId == realEstateId);
            var ap = await _repoRealEstate.AdPrices
                                    .Where(a => a.RealEstateId == realEstateId)
                                    .ToListAsync();
            result.RealEstateId = re.RealEstateId;
            result.AdId = re.AdId;
            result.Address = re.Address;
            result.City = re.City;
            result.Area = re.Area;
            result.PlotSize = re.PlotSize;
            var a = _mapper.Map<List<AdPriceDTO>>(ap);
            result.AdPrices =_mapper.Map<List<AdPriceDTO>>(ap);
            //foreach (var a in ap)
            //{

            //    var res = new AdPriceDTO()
            //    {
            //        Price = a.NewPrice,
            //        Created = a.EntryDate
            //    };
            //    result.AdPrices.Add(res);
            //}

            return result;
        }

        public async Task<IReadOnlyList<RealEstateDTO>> GetRealEstateLastChange(string estateType)
        {
            var lastFetchDate = await _repoRealEstate.FetchDates
                                    .OrderByDescending(f => f.EntryDate)
                                    .FirstOrDefaultAsync(f => f.EstateType == estateType);
            var adPrices = await _repoRealEstate.AdPrices
                                    .Where(r => r.FetchId == lastFetchDate.Id)
                                    .ToListAsync();
            var estateIds = adPrices.Select(a => a.RealEstateId).Distinct().ToList();

            var realEstates = await _repoRealEstate.RealEstates.Where(r => estateIds.Contains(r.RealEstateId)).ToListAsync();

            var result = new List<RealEstateDTO>();
            foreach (var re in realEstates)
            {
                var r = new RealEstateDTO();
                r.RealEstateId = re.RealEstateId;
                r.AdId = re.AdId;
                r.Address = re.Address;
                r.City = re.City;
                r.Area = re.Area;
                r.PlotSize = re.PlotSize;
                var ap = await _repoRealEstate.AdPrices.Where(a => a.RealEstateId == re.RealEstateId).ToListAsync();
                r.AdPrices = _mapper.Map<List<AdPriceDTO>>(ap);
                result.Add(r);
            }     
            
            return result;
        }
    }
}
