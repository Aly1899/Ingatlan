using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using restate.API.DTO;
using restate.Application.Contracts;

namespace restate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RealEstateController : ControllerBase
    {

        private readonly ILogger<RealEstateController> _logger;
        private readonly IRealEstateRepository _reRepo;

        public RealEstateController(ILogger<RealEstateController> logger, IRealEstateRepository reRepo)
        {
            _logger = logger;
            _reRepo = reRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(RealEstate), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<RealEstate>> GetRealEstates()
        {
            return await _reRepo.GetRealEstates();
        }

        [HttpGet("{realEstateId}")]
        [ProducesResponseType(typeof(RealEstate), (int)HttpStatusCode.OK)]
        public async Task<RealEstate> GetRealEstate(Guid realEstateId)
        {
            return await _reRepo.GetRealEstateById(realEstateId);
        }

        [HttpGet("/full/{realEstateId}")]
        [ProducesResponseType(typeof(RealEstate), (int)HttpStatusCode.OK)]
        public async Task<RealEstateDTO> GetRealEstateFull(Guid realEstateId)
        {
            return await _reRepo.GetRealEstateFull(realEstateId);
        }

        [HttpGet("/lastUpdated")]
        [ProducesResponseType(typeof(List<RealEstateDTO>), (int)HttpStatusCode.OK)]
        public async Task<IReadOnlyList<RealEstateDTO>> GetRealEstateLastChange(string estateType)
        {
            return await _reRepo.GetRealEstateLastChange(estateType);
        }
    }
}
