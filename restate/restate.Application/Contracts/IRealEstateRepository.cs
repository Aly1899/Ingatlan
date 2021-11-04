using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using restate.API.DTO;

namespace restate.Application.Contracts
{
    public interface IRealEstateRepository
    {
        public Task<IReadOnlyList<RealEstate>> GetRealEstates();
        public Task<RealEstate> GetRealEstateById(Guid realEstateId);
        public Task<RealEstateDTO> GetRealEstateFull(Guid realEstateId);
        public Task<IReadOnlyList<RealEstateDTO>> GetRealEstateLastChange(string estateType);

    }
}
