using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using reat.DTO;
using reat.Persistency.Entities;
using reat.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdController : ControllerBase
    {
        private readonly ILogger<AdController> _logger;
        private readonly IAdRepository<AdModel> _adRepository;
        private readonly IAdPriceRepository _adPriceRepository;
        private readonly IMapper _mapper;

        public AdController(
            ILogger<AdController> logger,
            IAdRepository<AdModel> adRepository,
            IAdPriceRepository adPriceRepository,
            IMapper mapper)
        {
            _logger = logger;
            _adRepository = adRepository;
            _adPriceRepository = adPriceRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAllAds")]
        public async Task<IReadOnlyList<GetAdsQueryResult>> Get()
        {
            var resAds = new List<GetAdsQueryResult>();
            var ads = await _adRepository.GetAllAds();
            foreach (var ad in ads)
            {
                var resAd = _mapper.Map<GetAdsQueryResult>(ad);
                resAd.AdPrices = (await _adPriceRepository.GetAllAdPrice()).Where(p => p.AdId == ad.AdId).ToList();
                resAds.Add(resAd);
            }

            return resAds;
        }

        [HttpGet("GetNewAds")]
        public async Task<IReadOnlyList<GetNewAdsQueryResult>> GetNewAds()
        {
            var resAds = new List<GetNewAdsQueryResult>();
            var ads = await _adRepository.GetNewAds();
            foreach (var ad in ads)
            {
                var resAd = _mapper.Map<GetNewAdsQueryResult>(ad);
                resAd.AdPrices = (await _adPriceRepository.GetAllAdPrice()).Where(p => p.AdId == ad.AdId).ToList();
                resAds.Add(resAd);
            }

            return resAds;
        }

        [HttpGet("GetNewInactiveAds")]
        public async Task<IReadOnlyList<GetNewInactiveAdsQueryResult>> GetNewInactiveAds()
        {
            var resAds = new List<GetNewInactiveAdsQueryResult>();
            var ads = await _adRepository.GetNewInactiveAds();
            foreach (var ad in ads)
            {
                var resAd = _mapper.Map<GetNewInactiveAdsQueryResult>(ad);
                resAd.AdPrices = (await _adPriceRepository.GetAllAdPrice()).Where(p => p.AdId == ad.AdId).ToList();
                resAds.Add(resAd);
            }

            return resAds;
        }
    }
}
